using MolServiceBusinessLogic.Helpers;
using MolServiceBusinessLogic.Models.OneC;
using MolServiceContracts.BindingModels;
using MolServiceContracts.BusinessLogicContracts;
using MolServiceContracts.SearchModels;
using MolServiceContracts.StorageContracts;
using MolServiceContracts.ViewModels;
using MolServiceDataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceBusinessLogic.Implements
{
    public class OneCImportLogic : IOneCImportLogic
    {
        private const string TargetLocation = "Кафедра ИС";

        private readonly OneCApiService _oneCApiService;
        private readonly IMaterialTechnicalValueStorage _materialTechnicalValueStorage;
        private readonly IMaterialResponsiblePersonStorage _materialResponsiblePersonStorage;

        public OneCImportLogic(
            OneCApiService oneCApiService,
            IMaterialTechnicalValueStorage materialTechnicalValueStorage,
            IMaterialResponsiblePersonStorage materialResponsiblePersonStorage)
        {
            _oneCApiService = oneCApiService;
            _materialTechnicalValueStorage = materialTechnicalValueStorage;
            _materialResponsiblePersonStorage = materialResponsiblePersonStorage;
        }

        public async Task<OneCImportResultViewModel> ImportFromOneCAsync(
            OneCImportBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrWhiteSpace(model.Username))
            {
                throw new ArgumentException("Не указан логин");
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                throw new ArgumentException("Не указан пароль");
            }

            var result = new OneCImportResultViewModel();

            var inventoryResponse = await _oneCApiService.GetInventoryAsync(
                model.Username,
                model.Password);

            var departmentMolsByKey = new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);

            var fixedAssetItems = inventoryResponse.Items
                .Where(x => !string.IsNullOrWhiteSpace(x.Code))
                .GroupBy(x => x.Code.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(x => ChooseBestInventoryItem(x))
                .ToList();

            foreach (var item in fixedAssetItems)
            {
                result.ImportedCount++;

                try
                {
                    if (string.IsNullOrWhiteSpace(item.Name))
                    {
                        throw new Exception("Пустое наименование объекта");
                    }

                    if (string.IsNullOrWhiteSpace(item.Code))
                    {
                        throw new Exception("Пустой код объекта");
                    }

                    var (molName, location) = ParseMolWithLocation(item.MolWithLocation);

                    if (!IsTargetLocation(location))
                    {
                        continue;
                    }

                    var molKey = BuildMolKey(molName);

                    if (!string.IsNullOrWhiteSpace(molKey) &&
                        !departmentMolsByKey.ContainsKey(molKey))
                    {
                        departmentMolsByKey.Add(molKey, molName);
                    }

                    UpsertFixedAsset(item, molName, location, result);
                }
                catch (Exception ex)
                {
                    result.ErrorCount++;
                    result.Errors.Add($"ОС. Код: {item.Code}. Ошибка: {ex.Message}");
                }
            }

            var materialStocksResponse = await _oneCApiService.GetMaterialStocksAsync(
                model.Username,
                model.Password);

            var stockItems = materialStocksResponse.Items
                .Where(x => !string.IsNullOrWhiteSpace(x.Code))
                .Where(x => !string.IsNullOrWhiteSpace(x.Mol))
                .Select(x => new
                {
                    Item = x,
                    MolKey = BuildMolKey(x.Mol)
                })
                .Where(x => departmentMolsByKey.ContainsKey(x.MolKey))
                .GroupBy(
                    x => BuildMaterialStockExternalKey(x.Item.Code, x.MolKey),
                    StringComparer.OrdinalIgnoreCase)
                .Select(group =>
                {
                    var first = group.First();

                    return new
                    {
                        first.Item,
                        first.MolKey,
                        Quantity = group.Sum(x => x.Item.Quantity)
                    };
                })
                .ToList();

            foreach (var stock in stockItems)
            {
                result.ImportedCount++;

                try
                {
                    if (string.IsNullOrWhiteSpace(stock.Item.Name))
                    {
                        throw new Exception("Пустое наименование материального запаса");
                    }

                    if (string.IsNullOrWhiteSpace(stock.Item.Code))
                    {
                        throw new Exception("Пустой код материального запаса");
                    }

                    var molNameFromFixedAssets = departmentMolsByKey[stock.MolKey];

                    UpsertMaterialStock(
                        stock.Item,
                        stock.Quantity,
                        molNameFromFixedAssets,
                        stock.MolKey,
                        result);
                }
                catch (Exception ex)
                {
                    result.ErrorCount++;
                    result.Errors.Add(
                        $"Материальные запасы. Код: {stock.Item.Code}. Ошибка: {ex.Message}");
                }
            }

            return result;
        }

        private void UpsertFixedAsset(
            OneCInventoryItem item,
            string molName,
            string location,
            OneCImportResultViewModel result)
        {
            var materialResponsiblePerson = GetOrCreateMaterialResponsiblePerson(molName);

            var externalKey = BuildFixedAssetExternalKey(item.Code);

            var existingMaterialTechnicalValue =
                _materialTechnicalValueStorage.GetElement(
                    new MaterialTechnicalValueSearchModel
                    {
                        ExternalKey = externalKey
                    });

            existingMaterialTechnicalValue ??=
                _materialTechnicalValueStorage.GetElement(
                    new MaterialTechnicalValueSearchModel
                    {
                        InventoryNumber = item.Code.Trim(),
                        SourceType = MaterialTechnicalValueSourceType.FixedAsset
                    });

            var bindingModel = new MaterialTechnicalValueBindingModel
            {
                Id = existingMaterialTechnicalValue?.Id ?? 0,
                InventoryNumber = item.Code.Trim(),
                ClassroomId = existingMaterialTechnicalValue?.ClassroomId,
                FullName = item.Name.Trim(),
                Quantity = 1,
                Description = item.Account ?? string.Empty,
                Location = location,
                MaterialResponsiblePersonId = materialResponsiblePerson.Id,
                SourceType = MaterialTechnicalValueSourceType.FixedAsset,
                ExternalKey = externalKey
            };

            if (existingMaterialTechnicalValue == null)
            {
                _materialTechnicalValueStorage.Insert(bindingModel);
                result.CreatedCount++;
            }
            else
            {
                _materialTechnicalValueStorage.Update(bindingModel);
                result.UpdatedCount++;
            }
        }

        private void UpsertMaterialStock(
            OneCMaterialStockItem item,
            decimal quantity,
            string molName,
            string molKey,
            OneCImportResultViewModel result)
        {
            var materialResponsiblePerson = GetOrCreateMaterialResponsiblePerson(molName);

            var externalKey = BuildMaterialStockExternalKey(item.Code, molKey);

            var existingMaterialTechnicalValue =
                _materialTechnicalValueStorage.GetElement(
                    new MaterialTechnicalValueSearchModel
                    {
                        ExternalKey = externalKey
                    });

            var bindingModel = new MaterialTechnicalValueBindingModel
            {
                Id = existingMaterialTechnicalValue?.Id ?? 0,
                InventoryNumber = item.Code.Trim(),
                ClassroomId = existingMaterialTechnicalValue?.ClassroomId,
                FullName = item.Name.Trim(),
                Quantity = quantity,
                Description = "Материальные запасы из 1С",
                Location = TargetLocation,
                MaterialResponsiblePersonId = materialResponsiblePerson.Id,
                SourceType = MaterialTechnicalValueSourceType.MaterialStock,
                ExternalKey = externalKey
            };

            if (existingMaterialTechnicalValue == null)
            {
                _materialTechnicalValueStorage.Insert(bindingModel);
                result.CreatedCount++;
            }
            else
            {
                _materialTechnicalValueStorage.Update(bindingModel);
                result.UpdatedCount++;
            }
        }

        private MaterialResponsiblePersonViewModel GetOrCreateMaterialResponsiblePerson(
            string fullName)
        {
            var materialResponsiblePerson =
                _materialResponsiblePersonStorage.GetElement(
                    new MaterialResponsiblePersonSearchModel
                    {
                        FullName = fullName
                    });

            if (materialResponsiblePerson == null)
            {
                materialResponsiblePerson =
                    _materialResponsiblePersonStorage.Insert(
                        new MaterialResponsiblePersonBindingModel
                        {
                            FullName = fullName,
                            Position = string.Empty,
                            Phone = string.Empty,
                            Email = string.Empty
                        });
            }

            if (materialResponsiblePerson == null)
            {
                throw new Exception("Не удалось создать или получить МОЛ");
            }

            return materialResponsiblePerson;
        }

        private static OneCInventoryItem ChooseBestInventoryItem(
            IGrouping<string, OneCInventoryItem> group)
        {
            return group
                .OrderByDescending(x => !string.IsNullOrWhiteSpace(x.Account))
                .First();
        }

        private static (string MolName, string Location) ParseMolWithLocation(
            string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return ("Неизвестный МОЛ", string.Empty);
            }

            var parts = value.Split('-', 2, StringSplitOptions.TrimEntries);

            if (parts.Length == 2)
            {
                return (parts[0].Trim(), parts[1].Trim());
            }

            return (value.Trim(), string.Empty);
        }

        private static bool IsTargetLocation(string location)
        {
            return NormalizeText(location) == NormalizeText(TargetLocation);
        }

        private static string BuildFixedAssetExternalKey(string code)
        {
            return $"inventoryNumbers:{NormalizeText(code)}";
        }

        private static string BuildMaterialStockExternalKey(
            string code,
            string molKey)
        {
            return $"matzp:{NormalizeText(code)}:{molKey}";
        }

        private static string BuildMolKey(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var parts = value
                .Replace(".", " ")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

            if (parts.Count == 0)
            {
                return string.Empty;
            }

            var lastName = NormalizeText(parts[0]);

            var firstInitial = parts.Count > 1 && !string.IsNullOrWhiteSpace(parts[1])
                ? NormalizeText(parts[1][0].ToString())
                : string.Empty;

            var patronymicInitial = parts.Count > 2 && !string.IsNullOrWhiteSpace(parts[2])
                ? NormalizeText(parts[2][0].ToString())
                : string.Empty;

            return $"{lastName}|{firstInitial}|{patronymicInitial}";
        }

        private static string NormalizeText(string? value)
        {
            return (value ?? string.Empty)
                .Trim()
                .Replace("ё", "е")
                .Replace("Ё", "Е")
                .ToLowerInvariant();
        }
    }
}
