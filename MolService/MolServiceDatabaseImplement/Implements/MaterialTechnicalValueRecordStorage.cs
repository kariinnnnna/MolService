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
    public class MaterialTechnicalValueRecordStorage : IMaterialTechnicalValueRecordStorage
    {
        private readonly MOLServiceDatabase _context;

        public MaterialTechnicalValueRecordStorage(MOLServiceDatabase context)
        {
            _context = context;
        }

        public List<MaterialTechnicalValueRecordViewModel> GetFullList()
        {
            return _context.MaterialTechnicalValueRecords
                .Include(x => x.MaterialTechnicalValueGroup)
                .Include(x => x.MaterialTechnicalValue)
                .Select(x => CreateModel(x))
                .ToList();
        }

        public List<MaterialTechnicalValueRecordViewModel> GetFilteredList(MaterialTechnicalValueRecordSearchModel model)
        {
            if (model == null)
            {
                return new();
            }

            var query = _context.MaterialTechnicalValueRecords
                .Include(x => x.MaterialTechnicalValueGroup)
                .Include(x => x.MaterialTechnicalValue)
                .AsQueryable();

            if (model.Id.HasValue)
            {
                query = query.Where(x => x.Id == model.Id.Value);
            }

            if (model.MaterialTechnicalValueGroupId.HasValue)
            {
                query = query.Where(x => x.MaterialTechnicalValueGroupId == model.MaterialTechnicalValueGroupId.Value);
            }

            if (model.MaterialTechnicalValueId.HasValue)
            {
                query = query.Where(x => x.MaterialTechnicalValueId == model.MaterialTechnicalValueId.Value);
            }

            if (!string.IsNullOrWhiteSpace(model.FieldName))
            {
                query = query.Where(x => x.FieldName.Contains(model.FieldName));
            }

            return query
                .Select(x => CreateModel(x))
                .ToList();
        }

        public MaterialTechnicalValueRecordViewModel? GetElement(MaterialTechnicalValueRecordSearchModel model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = _context.MaterialTechnicalValueRecords
                .Include(x => x.MaterialTechnicalValueGroup)
                .Include(x => x.MaterialTechnicalValue)
                .FirstOrDefault(x =>
                    (model.Id.HasValue && x.Id == model.Id.Value) ||
                    (model.MaterialTechnicalValueGroupId.HasValue && x.MaterialTechnicalValueGroupId == model.MaterialTechnicalValueGroupId.Value));

            return entity != null ? CreateModel(entity) : null;
        }

        public MaterialTechnicalValueRecordViewModel? Insert(MaterialTechnicalValueRecordBindingModel model)
        {
            var entity = new MaterialTechnicalValueRecord
            {
                MaterialTechnicalValueGroupId = model.MaterialTechnicalValueGroupId,
                MaterialTechnicalValueId = model.MaterialTechnicalValueId,
                FieldName = model.FieldName,
                FieldValue = model.FieldValue,
                Order = model.Order
            };

            _context.MaterialTechnicalValueRecords.Add(entity);
            _context.SaveChanges();

            return CreateModel(entity);
        }

        public MaterialTechnicalValueRecordViewModel? Update(MaterialTechnicalValueRecordBindingModel model)
        {
            var entity = _context.MaterialTechnicalValueRecords.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null)
            {
                return null;
            }

            entity.MaterialTechnicalValueGroupId = model.MaterialTechnicalValueGroupId;
            entity.MaterialTechnicalValueId = model.MaterialTechnicalValueId;
            entity.FieldName = model.FieldName;
            entity.FieldValue = model.FieldValue;
            entity.Order = model.Order;

            _context.SaveChanges();

            return CreateModel(entity);
        }

        public MaterialTechnicalValueRecordViewModel? Delete(MaterialTechnicalValueRecordBindingModel model)
        {
            var entity = _context.MaterialTechnicalValueRecords.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null)
            {
                return null;
            }

            var result = CreateModel(entity);
            _context.MaterialTechnicalValueRecords.Remove(entity);
            _context.SaveChanges();

            return result;
        }

        private static MaterialTechnicalValueRecordViewModel CreateModel(MaterialTechnicalValueRecord entity)
        {
            return new MaterialTechnicalValueRecordViewModel
            {
                Id = entity.Id,
                MaterialTechnicalValueGroupId = entity.MaterialTechnicalValueGroupId,
                MaterialTechnicalValueGroupName = entity.MaterialTechnicalValueGroup?.GroupName ?? string.Empty,
                MaterialTechnicalValueId = entity.MaterialTechnicalValueId,
                MaterialTechnicalValueName = entity.MaterialTechnicalValue?.FullName ?? string.Empty,
                FieldName = entity.FieldName,
                FieldValue = entity.FieldValue,
                Order = entity.Order
            };
        }
    }
}
