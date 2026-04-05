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
    public class MaterialTechnicalValueGroupStorage : IMaterialTechnicalValueGroupStorage
    {
        private readonly MOLServiceDatabase _context;

        public MaterialTechnicalValueGroupStorage(MOLServiceDatabase context)
        {
            _context = context;
        }

        public List<MaterialTechnicalValueGroupViewModel> GetFullList()
        {
            return _context.MaterialTechnicalValueGroups
                .Select(x => CreateModel(x))
                .ToList();
        }

        public List<MaterialTechnicalValueGroupViewModel> GetFilteredList(MaterialTechnicalValueGroupSearchModel model)
        {
            if (model == null)
            {
                return new();
            }

            var query = _context.MaterialTechnicalValueGroups.AsQueryable();

            if (model.Id.HasValue)
            {
                query = query.Where(x => x.Id == model.Id.Value);
            }

            if (!string.IsNullOrWhiteSpace(model.GroupName))
            {
                query = query.Where(x => x.GroupName.Contains(model.GroupName));
            }

            return query
                .Select(x => CreateModel(x))
                .ToList();
        }

        public MaterialTechnicalValueGroupViewModel? GetElement(MaterialTechnicalValueGroupSearchModel model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = _context.MaterialTechnicalValueGroups
                .FirstOrDefault(x => model.Id.HasValue && x.Id == model.Id.Value);

            return entity != null ? CreateModel(entity) : null;
        }

        public MaterialTechnicalValueGroupViewModel? Insert(MaterialTechnicalValueGroupBindingModel model)
        {
            var entity = new MaterialTechnicalValueGroup
            {
                GroupName = model.GroupName,
                Order = model.Order
            };

            _context.MaterialTechnicalValueGroups.Add(entity);
            _context.SaveChanges();

            return CreateModel(entity);
        }

        public MaterialTechnicalValueGroupViewModel? Update(MaterialTechnicalValueGroupBindingModel model)
        {
            var entity = _context.MaterialTechnicalValueGroups.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null)
            {
                return null;
            }

            entity.GroupName = model.GroupName;
            entity.Order = model.Order;

            _context.SaveChanges();

            return CreateModel(entity);
        }

        public MaterialTechnicalValueGroupViewModel? Delete(MaterialTechnicalValueGroupBindingModel model)
        {
            var entity = _context.MaterialTechnicalValueGroups.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null)
            {
                return null;
            }

            var result = CreateModel(entity);
            _context.MaterialTechnicalValueGroups.Remove(entity);
            _context.SaveChanges();

            return result;
        }

        private static MaterialTechnicalValueGroupViewModel CreateModel(MaterialTechnicalValueGroup entity)
        {
            return new MaterialTechnicalValueGroupViewModel
            {
                Id = entity.Id,
                GroupName = entity.GroupName,
                Order = entity.Order
            };
        }
    }
}
