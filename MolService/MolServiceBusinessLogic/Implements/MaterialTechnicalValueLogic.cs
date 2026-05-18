using MolServiceContracts.BindingModels;
using MolServiceContracts.BusinessLogicContracts;
using MolServiceContracts.SearchModels;
using MolServiceContracts.StorageContracts;
using MolServiceContracts.ViewModels;
using MolServiceDataModels.Enums;

namespace MolServiceBusinessLogic.Implements
{
    public class MaterialTechnicalValueLogic : IMaterialTechnicalValueLogic
    {
        private readonly IMaterialTechnicalValueStorage _storage;

        public MaterialTechnicalValueLogic(IMaterialTechnicalValueStorage storage)
        {
            _storage = storage;
        }

        public List<MaterialTechnicalValueViewModel>? ReadList(
            MaterialTechnicalValueSearchModel? model)
        {
            return model == null
                ? _storage.GetFullList()
                : _storage.GetFilteredList(model);
        }

        public PagedResultViewModel<MaterialTechnicalValueViewModel> ReadPagedList(
            MaterialTechnicalValueSearchModel model)
        {
            return _storage.GetPagedList(model);
        }

        public MaterialTechnicalValueViewModel? ReadElement(
            MaterialTechnicalValueSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return _storage.GetElement(model);
        }

        public MaterialTechnicalValueViewModel? Create(
            MaterialTechnicalValueBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrWhiteSpace(model.InventoryNumber))
            {
                throw new ArgumentException("Не указан инвентарный номер или код номенклатуры");
            }

            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                throw new ArgumentException("Не указано наименование");
            }

            if (model.Quantity <= 0)
            {
                throw new ArgumentException("Количество должно быть больше нуля");
            }

            if (model.SourceType == 0)
            {
                model.SourceType = MaterialTechnicalValueSourceType.FixedAsset;
            }

            model.ExternalKey ??= string.Empty;
            model.Location = string.IsNullOrWhiteSpace(model.Location)
                ? "Кафедра ИС"
                : model.Location.Trim();

            var existingByInventoryNumber = _storage.GetElement(
                new MaterialTechnicalValueSearchModel
                {
                    InventoryNumber = model.InventoryNumber.Trim(),
                    SourceType = model.SourceType
                });

            if (existingByInventoryNumber != null)
            {
                throw new InvalidOperationException(
                    "Оборудование или материальный запас с таким номером уже существует");
            }

            return _storage.Insert(model);
        }

        public MaterialTechnicalValueViewModel? Update(
            MaterialTechnicalValueBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор оборудования");
            }

            if (string.IsNullOrWhiteSpace(model.InventoryNumber))
            {
                throw new ArgumentException("Не указан инвентарный номер или код номенклатуры");
            }

            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                throw new ArgumentException("Не указано наименование");
            }

            if (model.Quantity <= 0)
            {
                throw new ArgumentException("Количество должно быть больше нуля");
            }

            var element = _storage.GetElement(
                new MaterialTechnicalValueSearchModel
                {
                    Id = model.Id
                });

            if (element == null)
            {
                throw new InvalidOperationException("Оборудование не найдено");
            }

            model.ExternalKey ??= element.ExternalKey;
            model.SourceType = model.SourceType == 0
                ? element.SourceType
                : model.SourceType;

            model.Location = string.IsNullOrWhiteSpace(model.Location)
                ? "Кафедра ИС"
                : model.Location.Trim();

            return _storage.Update(model);
        }

        public bool Delete(MaterialTechnicalValueBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор оборудования");
            }

            var element = _storage.GetElement(
                new MaterialTechnicalValueSearchModel
                {
                    Id = model.Id
                });

            if (element == null)
            {
                throw new InvalidOperationException("Оборудование не найдено");
            }

            return _storage.Delete(model) != null;
        }
    }
}