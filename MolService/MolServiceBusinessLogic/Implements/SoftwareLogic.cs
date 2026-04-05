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
    public class SoftwareLogic : ISoftwareLogic
    {
        private readonly ISoftwareStorage _storage;

        public SoftwareLogic(ISoftwareStorage storage)
        {
            _storage = storage;
        }

        public List<SoftwareViewModel>? ReadList(SoftwareSearchModel? model)
        {
            return model == null
                ? _storage.GetFullList()
                : _storage.GetFilteredList(model);
        }

        public SoftwareViewModel? ReadElement(SoftwareSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return _storage.GetElement(model);
        }

        public SoftwareViewModel? Create(SoftwareBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrWhiteSpace(model.SoftwareName))
            {
                throw new ArgumentException("Не указано название программного обеспечения");
            }

            var existingSoftware = _storage.GetElement(new SoftwareSearchModel
            {
                SoftwareName = model.SoftwareName
            });

            if (existingSoftware != null)
            {
                throw new InvalidOperationException("Программное обеспечение с таким названием уже существует");
            }

            return _storage.Insert(model);
        }

        public SoftwareViewModel? Update(SoftwareBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор ПО");
            }

            var element = _storage.GetElement(new SoftwareSearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Программное обеспечение не найдено");
            }

            return _storage.Update(model);
        }

        public bool Delete(SoftwareBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id <= 0)
            {
                throw new ArgumentException("Не указан идентификатор ПО");
            }

            var element = _storage.GetElement(new SoftwareSearchModel
            {
                Id = model.Id
            });

            if (element == null)
            {
                throw new InvalidOperationException("Программное обеспечение не найдено");
            }

            return _storage.Delete(model) != null;
        }
    }
}
