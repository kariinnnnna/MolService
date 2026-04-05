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
    public class SoftwareRecordLogic : ISoftwareRecordLogic
    {
        private readonly ISoftwareRecordStorage _storage;

        public SoftwareRecordLogic(ISoftwareRecordStorage storage)
        {
            _storage = storage;
        }

        public List<SoftwareRecordViewModel>? ReadList(SoftwareRecordSearchModel? model)
        {
            return model == null
                ? _storage.GetFullList()
                : _storage.GetFilteredList(model);
        }

        public SoftwareRecordViewModel? ReadElement(SoftwareRecordSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return _storage.GetElement(model);
        }

        public SoftwareRecordViewModel? Create(SoftwareRecordBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrWhiteSpace(model.SetupDescription))
            {
                throw new ArgumentException("Не указано описание настройки ПО");
            }

            return _storage.Insert(model);
        }

        public SoftwareRecordViewModel? Update(SoftwareRecordBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор записи ПО");
            }

            var element = _storage.GetElement(new SoftwareRecordSearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Запись ПО не найдена");
            }

            return _storage.Update(model);
        }

        public bool Delete(SoftwareRecordBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор записи ПО");
            }

            var element = _storage.GetElement(new SoftwareRecordSearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Запись ПО не найдена");
            }

            return _storage.Delete(model) != null;
        }
    }
}
