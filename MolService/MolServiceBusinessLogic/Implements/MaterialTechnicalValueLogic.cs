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
    public class MaterialTechnicalValueLogic : IMaterialTechnicalValueLogic
    {
        private readonly IMaterialTechnicalValueStorage _storage;

        public MaterialTechnicalValueLogic(IMaterialTechnicalValueStorage storage)
        {
            _storage = storage;
        }

        public List<MaterialTechnicalValueViewModel>? ReadList(MaterialTechnicalValueSearchModel? model)
        {
            return model == null
                ? _storage.GetFullList()
                : _storage.GetFilteredList(model);
        }

        public MaterialTechnicalValueViewModel? ReadElement(MaterialTechnicalValueSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return _storage.GetElement(model);
        }

        public MaterialTechnicalValueViewModel? Create(MaterialTechnicalValueBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrWhiteSpace(model.InventoryNumber))
            {
                throw new ArgumentException("Не указан инвентарный номер");
            }

            model.Location = "Кафедра ИС";

            var existingByInventoryNumber = _storage.GetElement(new MaterialTechnicalValueSearchModel
            {
                InventoryNumber = model.InventoryNumber
            });

            if (existingByInventoryNumber != null)
            {
                throw new InvalidOperationException("Оборудование с таким инвентарным номером уже существует");
            }

            return _storage.Insert(model);
        }

        public MaterialTechnicalValueViewModel? Update(MaterialTechnicalValueBindingModel model)
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
                throw new ArgumentException("Не указан инвентарный номер");
            }

            var element = _storage.GetElement(new MaterialTechnicalValueSearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Оборудование не найдено");
            }

            model.Location = "Кафедра ИС";

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

            var element = _storage.GetElement(new MaterialTechnicalValueSearchModel
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
