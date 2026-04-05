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
    public class MaterialTechnicalValueRecordLogic : IMaterialTechnicalValueRecordLogic
    {
        private readonly IMaterialTechnicalValueRecordStorage _storage;

        public MaterialTechnicalValueRecordLogic(IMaterialTechnicalValueRecordStorage storage)
        {
            _storage = storage;
        }

        public List<MaterialTechnicalValueRecordViewModel>? ReadList(MaterialTechnicalValueRecordSearchModel? model)
        {
            return model == null
                ? _storage.GetFullList()
                : _storage.GetFilteredList(model);
        }

        public MaterialTechnicalValueRecordViewModel? ReadElement(MaterialTechnicalValueRecordSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return _storage.GetElement(model);
        }

        public MaterialTechnicalValueRecordViewModel? Create(MaterialTechnicalValueRecordBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrWhiteSpace(model.FieldName))
            {
                throw new ArgumentException("Не указано имя поля");
            }

            return _storage.Insert(model);
        }

        public MaterialTechnicalValueRecordViewModel? Update(MaterialTechnicalValueRecordBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор записи");
            }

            var element = _storage.GetElement(new MaterialTechnicalValueRecordSearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Запись не найдена");
            }

            return _storage.Update(model);
        }

        public bool Delete(MaterialTechnicalValueRecordBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор записи");
            }

            var element = _storage.GetElement(new MaterialTechnicalValueRecordSearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Запись не найдена");
            }

            return _storage.Delete(model) != null;
        }
    }
}
