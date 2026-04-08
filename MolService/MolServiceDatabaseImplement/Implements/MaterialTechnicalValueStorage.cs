using Microsoft.EntityFrameworkCore;
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
    public class MaterialTechnicalValueStorage : IMaterialTechnicalValueStorage
    {
        private readonly MOLServiceDatabase _context;

        public MaterialTechnicalValueStorage(MOLServiceDatabase context)
        {
            _context = context;
        }

        public List<MaterialTechnicalValueViewModel> GetFullList()
        {
            return _context.MaterialTechnicalValues
                .Include(x => x.Classroom)
                .Include(x => x.MaterialResponsiblePerson)
                .Select(x => CreateModel(x))
                .ToList();
        }

        public List<MaterialTechnicalValueViewModel> GetFilteredList(MaterialTechnicalValueSearchModel model)
        {
            if (model == null)
            {
                return new();
            }

            var query = _context.MaterialTechnicalValues
                .Include(x => x.Classroom)
                .Include(x => x.MaterialResponsiblePerson)
                .AsQueryable();

            if (model.Id.HasValue)
            {
                query = query.Where(x => x.Id == model.Id.Value);
            }

            if (!string.IsNullOrWhiteSpace(model.InventoryNumber))
            {
                query = query.Where(x => x.InventoryNumber.Contains(model.InventoryNumber));
            }

            if (model.ClassroomId.HasValue)
            {
                query = query.Where(x => x.ClassroomId == model.ClassroomId.Value);
            }

            if (!string.IsNullOrWhiteSpace(model.FullName))
            {
                query = query.Where(x => x.FullName.Contains(model.FullName));
            }

            if (!string.IsNullOrWhiteSpace(model.Location))
            {
                query = query.Where(x => x.Location.Contains(model.Location));
            }

            return query
                .Select(x => CreateModel(x))
                .ToList();
        }

        public MaterialTechnicalValueViewModel? GetElement(MaterialTechnicalValueSearchModel model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = _context.MaterialTechnicalValues
                .Include(x => x.Classroom)
                .Include(x => x.MaterialResponsiblePerson)
                .FirstOrDefault(x =>
                    (model.Id.HasValue && x.Id == model.Id.Value) ||
                    (model.InventoryNumber != null && x.InventoryNumber == model.InventoryNumber));

            return entity != null ? CreateModel(entity) : null;
        }

        public MaterialTechnicalValueViewModel? Insert(MaterialTechnicalValueBindingModel model)
        {
            var entity = new MaterialTechnicalValue
            {
                InventoryNumber = model.InventoryNumber,
                ClassroomId = model.ClassroomId,
                FullName = model.FullName,
                Quantity = model.Quantity,
                Description = model.Description,
                Location = model.Location,
                MaterialResponsiblePersonId = model.MaterialResponsiblePersonId
            };

            _context.MaterialTechnicalValues.Add(entity);
            _context.SaveChanges();

            return CreateModel(entity);
        }

        public MaterialTechnicalValueViewModel? Update(MaterialTechnicalValueBindingModel model)
        {
            var entity = _context.MaterialTechnicalValues.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null)
            {
                return null;
            }

            entity.InventoryNumber = model.InventoryNumber;
            entity.ClassroomId = model.ClassroomId;
            entity.FullName = model.FullName;
            entity.Quantity = model.Quantity;
            entity.Description = model.Description;
            entity.Location = model.Location;
            entity.MaterialResponsiblePersonId = model.MaterialResponsiblePersonId;

            _context.SaveChanges();

            return CreateModel(entity);
        }

        public MaterialTechnicalValueViewModel? Delete(MaterialTechnicalValueBindingModel model)
        {
            var entity = _context.MaterialTechnicalValues.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null)
            {
                return null;
            }

            var result = CreateModel(entity);
            _context.MaterialTechnicalValues.Remove(entity);
            _context.SaveChanges();

            return result;
        }

        private static MaterialTechnicalValueViewModel CreateModel(MaterialTechnicalValue entity)
        {
            return new MaterialTechnicalValueViewModel
            {
                Id = entity.Id,
                InventoryNumber = entity.InventoryNumber,
                ClassroomId = entity.ClassroomId,
                ClassroomNumber = entity.Classroom?.Number ?? string.Empty,
                FullName = entity.FullName,
                Quantity = entity.Quantity,
                Description = entity.Description,
                Location = entity.Location,
                MaterialResponsiblePersonId = entity.MaterialResponsiblePersonId,
                MaterialResponsiblePersonName = entity.MaterialResponsiblePerson?.FullName ?? string.Empty
            };
        }
    }
}
