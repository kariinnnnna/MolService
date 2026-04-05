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
    public class MaterialResponsiblePersonLogic : IMaterialResponsiblePersonLogic
    {
        private readonly IMaterialResponsiblePersonStorage _storage;

        public MaterialResponsiblePersonLogic(IMaterialResponsiblePersonStorage storage)
        {
            _storage = storage;
        }

        public List<MaterialResponsiblePersonViewModel>? ReadList(MaterialResponsiblePersonSearchModel? model)
        {
            return model == null
                ? _storage.GetFullList()
                : _storage.GetFilteredList(model);
        }

        public MaterialResponsiblePersonViewModel? ReadElement(MaterialResponsiblePersonSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return _storage.GetElement(model);
        }

        public MaterialResponsiblePersonViewModel? Create(MaterialResponsiblePersonBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                throw new ArgumentException("Не указано полное имя ответственного лица");
            }

            var existingPerson = _storage.GetElement(new MaterialResponsiblePersonSearchModel
            {
                FullName = model.FullName
            });

            if (existingPerson != null)
            {
                throw new InvalidOperationException("Ответственное лицо с таким именем уже существует");
            }

            return _storage.Insert(model);
        }

        public MaterialResponsiblePersonViewModel? Update(MaterialResponsiblePersonBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор ответственного лица");
            }

            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                throw new ArgumentException("Не указано полное имя ответственного лица");
            }

            var element = _storage.GetElement(new MaterialResponsiblePersonSearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Ответственное лицо не найдено");
            }

            return _storage.Update(model);
        }

        public bool Delete(MaterialResponsiblePersonBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор ответственного лица");
            }

            var element = _storage.GetElement(new MaterialResponsiblePersonSearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Ответственное лицо не найдено");
            }

            return _storage.Delete(model) != null;
        }
    }
}
