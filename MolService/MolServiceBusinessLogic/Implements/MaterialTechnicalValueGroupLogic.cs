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
    public class MaterialTechnicalValueGroupLogic : IMaterialTechnicalValueGroupLogic
    {
        private readonly IMaterialTechnicalValueGroupStorage _storage;

        public MaterialTechnicalValueGroupLogic(IMaterialTechnicalValueGroupStorage storage)
        {
            _storage = storage;
        }

        public List<MaterialTechnicalValueGroupViewModel>? ReadList(MaterialTechnicalValueGroupSearchModel? model)
        {
            return model == null
                ? _storage.GetFullList()
                : _storage.GetFilteredList(model);
        }

        public MaterialTechnicalValueGroupViewModel? ReadElement(MaterialTechnicalValueGroupSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return _storage.GetElement(model);
        }

        public MaterialTechnicalValueGroupViewModel? Create(MaterialTechnicalValueGroupBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrWhiteSpace(model.GroupName))
            {
                throw new ArgumentException("Не указано имя группы технического материала");
            }

            return _storage.Insert(model);
        }

        public MaterialTechnicalValueGroupViewModel? Update(MaterialTechnicalValueGroupBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор группы");
            }

            var element = _storage.GetElement(new MaterialTechnicalValueGroupSearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Группа технического материала не найдена");
            }

            return _storage.Update(model);
        }

        public bool Delete(MaterialTechnicalValueGroupBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор группы");
            }

            var element = _storage.GetElement(new MaterialTechnicalValueGroupSearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Группа технического материала не найдена");
            }

            return _storage.Delete(model) != null;
        }
    }
}
