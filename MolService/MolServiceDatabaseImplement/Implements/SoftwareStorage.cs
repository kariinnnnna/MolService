using MolServiceContracts.BindingModels;
using MolServiceContracts.SearchModels;
using MolServiceContracts.StorageContracts;
using MolServiceContracts.ViewModels;
using MolServiceDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDatabaseImplement.Implements
{
    public class SoftwareStorage : ISoftwareStorage
    {
        private readonly MOLServiceDatabase _context;

        public SoftwareStorage(MOLServiceDatabase context)
        {
            _context = context;
        }

        public List<SoftwareViewModel> GetFullList()
        {
            return _context.Softwares
                .Select(x => CreateModel(x))
                .ToList();
        }

        public List<SoftwareViewModel> GetFilteredList(SoftwareSearchModel model)
        {
            if (model == null)
            {
                return new();
            }

            var query = _context.Softwares.AsQueryable();

            if (model.Id.HasValue)
            {
                query = query.Where(x => x.Id == model.Id.Value);
            }

            if (!string.IsNullOrWhiteSpace(model.SoftwareName))
            {
                query = query.Where(x => x.SoftwareName.Contains(model.SoftwareName));
            }

            if (!string.IsNullOrWhiteSpace(model.SoftwareKey))
            {
                query = query.Where(x => x.SoftwareKey.Contains(model.SoftwareKey));
            }

            return query
                .Select(x => CreateModel(x))
                .ToList();
        }

        public SoftwareViewModel? GetElement(SoftwareSearchModel model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = _context.Softwares.FirstOrDefault(x =>
                (model.Id.HasValue && x.Id == model.Id.Value) ||
                (!string.IsNullOrEmpty(model.SoftwareName) && x.SoftwareName.Contains(model.SoftwareName)));

            return entity != null ? CreateModel(entity) : null;
        }

        public SoftwareViewModel? Insert(SoftwareBindingModel model)
        {
            var entity = new Software
            {
                SoftwareName = model.SoftwareName,
                SoftwareDescription = model.SoftwareDescription,
                SoftwareKey = model.SoftwareKey,
                SoftwareK = model.SoftwareK
            };

            _context.Softwares.Add(entity);
            _context.SaveChanges();

            return CreateModel(entity);
        }

        public SoftwareViewModel? Update(SoftwareBindingModel model)
        {
            var entity = _context.Softwares.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null)
            {
                return null;
            }

            entity.SoftwareName = model.SoftwareName;
            entity.SoftwareDescription = model.SoftwareDescription;
            entity.SoftwareKey = model.SoftwareKey;
            entity.SoftwareK = model.SoftwareK;

            _context.SaveChanges();

            return CreateModel(entity);
        }

        public SoftwareViewModel? Delete(SoftwareBindingModel model)
        {
            var entity = _context.Softwares.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null)
            {
                return null;
            }

            var result = CreateModel(entity);
            _context.Softwares.Remove(entity);
            _context.SaveChanges();

            return result;
        }

        private static SoftwareViewModel CreateModel(Software entity)
        {
            return new SoftwareViewModel
            {
                Id = entity.Id,
                SoftwareName = entity.SoftwareName,
                SoftwareDescription = entity.SoftwareDescription,
                SoftwareKey = entity.SoftwareKey,
                SoftwareK = entity.SoftwareK
            };
        }
    }
}
