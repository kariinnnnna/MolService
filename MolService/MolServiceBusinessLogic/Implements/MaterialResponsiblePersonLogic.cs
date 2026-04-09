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

            ValidateModel(model);

            CheckDuplicates(model);

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

            ValidateModel(model);

            var element = _storage.GetElement(new MaterialResponsiblePersonSearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Ответственное лицо не найдено");
            }

            CheckDuplicates(model);

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

        private static void ValidateModel(MaterialResponsiblePersonBindingModel model)
        {
            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                throw new ArgumentException("Не указано полное имя ответственного лица");
            }

            model.FullName = model.FullName.Trim();

            if (!string.IsNullOrWhiteSpace(model.Phone))
            {
                model.Phone = model.Phone.Trim();
            }

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                model.Email = model.Email.Trim();
            }
        }

        private void CheckDuplicates(MaterialResponsiblePersonBindingModel model)
        {
            var sameFullName = _storage.GetElement(new MaterialResponsiblePersonSearchModel
            {
                FullName = model.FullName
            });

            if (sameFullName != null && sameFullName.Id != model.Id)
            {
                throw new InvalidOperationException("Ответственное лицо с таким ФИО уже существует");
            }

            if (!string.IsNullOrWhiteSpace(model.Phone))
            {
                var samePhone = _storage.GetElement(new MaterialResponsiblePersonSearchModel
                {
                    Phone = model.Phone
                });

                if (samePhone != null && samePhone.Id != model.Id)
                {
                    throw new InvalidOperationException("Ответственное лицо с таким телефоном уже существует");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var sameEmail = _storage.GetElement(new MaterialResponsiblePersonSearchModel
                {
                    Email = model.Email
                });

                if (sameEmail != null && sameEmail.Id != model.Id)
                {
                    throw new InvalidOperationException("Ответственное лицо с таким email уже существует");
                }
            }
        }
    }
}
