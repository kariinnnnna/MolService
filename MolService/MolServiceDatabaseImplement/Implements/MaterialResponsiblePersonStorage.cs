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
    public class MaterialResponsiblePersonStorage : IMaterialResponsiblePersonStorage
    {
        private readonly MOLServiceDatabase _context;

        public MaterialResponsiblePersonStorage(MOLServiceDatabase context)
        {
            _context = context;
        }

        public List<MaterialResponsiblePersonViewModel> GetFullList()
        {
            return _context.MaterialResponsiblePersons
                .Select(x => CreateModel(x))
                .ToList();
        }

        public List<MaterialResponsiblePersonViewModel> GetFilteredList(MaterialResponsiblePersonSearchModel model)
        {
            if (model == null)
            {
                return new();
            }

            var query = _context.MaterialResponsiblePersons.AsQueryable();

            if (model.Id.HasValue)
            {
                query = query.Where(x => x.Id == model.Id.Value);
            }

            if (!string.IsNullOrWhiteSpace(model.FullName))
            {
                query = query.Where(x => x.FullName.Contains(model.FullName));
            }

            if (!string.IsNullOrWhiteSpace(model.Position))
            {
                query = query.Where(x => x.Position.Contains(model.Position));
            }

            if (!string.IsNullOrWhiteSpace(model.Phone))
            {
                query = query.Where(x => x.Phone.Contains(model.Phone));
            }

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                query = query.Where(x => x.Email.Contains(model.Email));
            }

            return query
                .Select(x => CreateModel(x))
                .ToList();
        }

        public MaterialResponsiblePersonViewModel? GetElement(MaterialResponsiblePersonSearchModel model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = _context.MaterialResponsiblePersons.FirstOrDefault(x =>
                (model.Id.HasValue && x.Id == model.Id.Value) ||
                (!string.IsNullOrWhiteSpace(model.FullName) && x.FullName == model.FullName));

            return entity != null ? CreateModel(entity) : null;
        }

        public MaterialResponsiblePersonViewModel? Insert(MaterialResponsiblePersonBindingModel model)
        {
            var entity = new MaterialResponsiblePerson
            {
                FullName = model.FullName,
                Position = model.Position,
                Phone = model.Phone,
                Email = model.Email
            };

            _context.MaterialResponsiblePersons.Add(entity);
            _context.SaveChanges();

            return CreateModel(entity);
        }

        public MaterialResponsiblePersonViewModel? Update(MaterialResponsiblePersonBindingModel model)
        {
            var entity = _context.MaterialResponsiblePersons.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null)
            {
                return null;
            }

            entity.FullName = model.FullName;
            entity.Position = model.Position;
            entity.Phone = model.Phone;
            entity.Email = model.Email;

            _context.SaveChanges();

            return CreateModel(entity);
        }

        public MaterialResponsiblePersonViewModel? Delete(MaterialResponsiblePersonBindingModel model)
        {
            var entity = _context.MaterialResponsiblePersons.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null)
            {
                return null;
            }

            var result = CreateModel(entity);
            _context.MaterialResponsiblePersons.Remove(entity);
            _context.SaveChanges();

            return result;
        }

        private static MaterialResponsiblePersonViewModel CreateModel(MaterialResponsiblePerson entity)
        {
            return new MaterialResponsiblePersonViewModel
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Position = entity.Position,
                Phone = entity.Phone,
                Email = entity.Email
            };
        }
    }
}
