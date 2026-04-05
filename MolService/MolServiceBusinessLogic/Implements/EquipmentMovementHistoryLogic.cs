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
    public class EquipmentMovementHistoryLogic : IEquipmentMovementHistoryLogic
    {
        private readonly IEquipmentMovementHistoryStorage _storage;

        public EquipmentMovementHistoryLogic(IEquipmentMovementHistoryStorage storage)
        {
            _storage = storage;
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

            if (model.MoveDate == null)
            {
                throw new ArgumentException("Не указана дата перемещения оборудования");
            }

            return _storage.Insert(model);
        }

        public EquipmentMovementHistoryViewModel? Update(EquipmentMovementHistoryBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор перемещения оборудования");
            }

            var element = _storage.GetElement(new EquipmentMovementHistorySearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Перемещение оборудования не найдено");
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
                throw new ArgumentException("Не указан идентификатор перемещения оборудования");
            }

            var element = _storage.GetElement(new EquipmentMovementHistorySearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Перемещение оборудования не найдено");
            }

            return _storage.Delete(model) != null;
        }
    }
}
