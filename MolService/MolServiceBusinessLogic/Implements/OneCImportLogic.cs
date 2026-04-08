using MolServiceBusinessLogic.Helpers;
using MolServiceContracts.BindingModels;
using MolServiceContracts.BusinessLogicContracts;
using MolServiceContracts.SearchModels;
using MolServiceContracts.StorageContracts;
using MolServiceContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceBusinessLogic.Implements
{
    public class OneCImportLogic : IOneCImportLogic
    {
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

        public async Task<OneCImportResultViewModel> ImportFromOneCAsync(OneCImportBindingModel model)
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

            var response = await _oneCApiService.GetInventoryAsync(model.Username, model.Password);

            foreach (var item in response.Items)
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

                    var materialResponsiblePerson =
                        _materialResponsiblePersonStorage.GetElement(new MaterialResponsiblePersonSearchModel
                        {
                            FullName = molName
                        });

                    if (materialResponsiblePerson == null)
                    {
                        materialResponsiblePerson = _materialResponsiblePersonStorage.Insert(new MaterialResponsiblePersonBindingModel
                        {
                            FullName = molName,
                            Position = string.Empty,
                            Phone = string.Empty,
                            Email = string.Empty
                        });
                    }

                    if (materialResponsiblePerson == null)
                    {
                        throw new Exception("Не удалось создать или получить МОЛ");
                    }

                    var existingMaterialTechnicalValue =
                        _materialTechnicalValueStorage.GetElement(new MaterialTechnicalValueSearchModel
                        {
                            InventoryNumber = item.Code
                        });

                    if (existingMaterialTechnicalValue == null)
                    {
                        _materialTechnicalValueStorage.Insert(new MaterialTechnicalValueBindingModel
                        {
                            InventoryNumber = item.Code,
                            ClassroomId = null,
                            FullName = item.Name,
                            Quantity = 1,
                            Description = item.Account ?? string.Empty,
                            Location = location,
                            MaterialResponsiblePersonId = materialResponsiblePerson.Id
                        });

                        result.CreatedCount++;
                    }
                    else
                    {
                        _materialTechnicalValueStorage.Update(new MaterialTechnicalValueBindingModel
                        {
                            Id = existingMaterialTechnicalValue.Id,
                            InventoryNumber = item.Code,
                            ClassroomId = existingMaterialTechnicalValue.ClassroomId,
                            FullName = item.Name,
                            Quantity = 1,
                            Description = item.Account ?? string.Empty,
                            Location = location,
                            MaterialResponsiblePersonId = materialResponsiblePerson.Id
                        });

                        result.UpdatedCount++;
                    }
                }
                catch (Exception ex)
                {
                    result.ErrorCount++;
                    result.Errors.Add($"Код: {item.Code}. Ошибка: {ex.Message}");
                }
            }

            return result;
        }

        private static (string MolName, string Location) ParseMolWithLocation(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return ("Неизвестный МОЛ", string.Empty);
            }

            var parts = value.Split(" - ", 2, StringSplitOptions.TrimEntries);

            if (parts.Length == 2)
            {
                return (parts[0], parts[1]);
            }

            return (value.Trim(), string.Empty);
        }
    }
}
