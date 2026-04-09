using MolServiceContracts.BindingModels;
using MolServiceContracts.BusinessLogicContracts;
using MolServiceContracts.SearchModels;
using MolServiceContracts.StorageContracts;
using MolServiceContracts.ViewModels;

namespace MolServiceBusinessLogic.Implements
{
    public class EquipmentMovementHistoryLogic : IEquipmentMovementHistoryLogic
    {
        private readonly IEquipmentMovementHistoryStorage _storage;
        private readonly IMaterialTechnicalValueStorage _materialTechnicalValueStorage;

        public EquipmentMovementHistoryLogic(
            IEquipmentMovementHistoryStorage storage,
            IMaterialTechnicalValueStorage materialTechnicalValueStorage)
        {
            _storage = storage;
            _materialTechnicalValueStorage = materialTechnicalValueStorage;
        }

        public List<EquipmentMovementHistoryViewModel>? ReadList(EquipmentMovementHistorySearchModel? model)
        {
            return model == null
                ? _storage.GetFullList()
                : _storage.GetFilteredList(model);
        }

        public EquipmentMovementHistoryViewModel? ReadElement(EquipmentMovementHistorySearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return _storage.GetElement(model);
        }

        public EquipmentMovementHistoryViewModel? Create(EquipmentMovementHistoryBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.MaterialTechnicalValueId <= 0)
            {
                throw new ArgumentException("Не указано оборудование для списания");
            }

            if (model.MoveDate == default)
            {
                throw new ArgumentException("Не указана дата списания оборудования");
            }

            if (string.IsNullOrWhiteSpace(model.Reason))
            {
                throw new ArgumentException("Не указана причина списания оборудования");
            }

            if (model.Quantity <= 0)
            {
                throw new ArgumentException("Количество для списания должно быть больше 0");
            }

            var materialTechnicalValue = _materialTechnicalValueStorage.GetElement(
                new MaterialTechnicalValueSearchModel
                {
                    Id = model.MaterialTechnicalValueId
                });

            if (materialTechnicalValue == null)
            {
                throw new InvalidOperationException("Оборудование не найдено");
            }

            if (model.Quantity > materialTechnicalValue.Quantity)
            {
                throw new ArgumentException("Нельзя списать больше, чем есть в наличии");
            }

            var result = _storage.Insert(model);

            var newQuantity = materialTechnicalValue.Quantity - model.Quantity;

            var updateModel = new MaterialTechnicalValueBindingModel
            {
                Id = materialTechnicalValue.Id,
                InventoryNumber = materialTechnicalValue.InventoryNumber,
                ClassroomId = newQuantity == 0 ? null : materialTechnicalValue.ClassroomId,
                FullName = materialTechnicalValue.FullName,
                Quantity = newQuantity,
                Description = materialTechnicalValue.Description,
                Location = materialTechnicalValue.Location,
                MaterialResponsiblePersonId = materialTechnicalValue.MaterialResponsiblePersonId
            };

            _materialTechnicalValueStorage.Update(updateModel);

            return result;
        }

        public EquipmentMovementHistoryViewModel? Update(EquipmentMovementHistoryBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор записи списания");
            }

            if (model.MaterialTechnicalValueId <= 0)
            {
                throw new ArgumentException("Не указано оборудование");
            }

            if (model.MoveDate == default)
            {
                throw new ArgumentException("Не указана дата списания оборудования");
            }

            if (string.IsNullOrWhiteSpace(model.Reason))
            {
                throw new ArgumentException("Не указана причина списания оборудования");
            }

            if (model.Quantity <= 0)
            {
                throw new ArgumentException("Количество для списания должно быть больше 0");
            }

            var element = _storage.GetElement(new EquipmentMovementHistorySearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Запись списания не найдена");
            }

            return _storage.Update(model);
        }

        public bool Delete(EquipmentMovementHistoryBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор записи списания");
            }

            var element = _storage.GetElement(new EquipmentMovementHistorySearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Запись списания не найдена");
            }

            return _storage.Delete(model) != null;
        }
    }
}