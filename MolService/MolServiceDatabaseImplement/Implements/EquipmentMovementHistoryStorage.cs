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
    public class EquipmentMovementHistoryStorage : IEquipmentMovementHistoryStorage
    {
        private readonly MOLServiceDatabase _context;

        public EquipmentMovementHistoryStorage(MOLServiceDatabase context)
        {
            _context = context;
        }

        public List<EquipmentMovementHistoryViewModel> GetFullList()
        {
            return _context.EquipmentMovementHistories
                .Include(x => x.MaterialTechnicalValue)
                .Select(x => CreateModel(x))
                .ToList();
        }

        public List<EquipmentMovementHistoryViewModel> GetFilteredList(EquipmentMovementHistorySearchModel model)
        {
            if (model == null)
            {
                return new();
            }

            var query = _context.EquipmentMovementHistories
                .Include(x => x.MaterialTechnicalValue)
                .AsQueryable();

            if (model.MoveDateFrom.HasValue)
            {
                query = query.Where(x => x.MoveDate >= model.MoveDateFrom.Value);
            }

            if (model.MoveDateTo.HasValue)
            {
                query = query.Where(x => x.MoveDate <= model.MoveDateTo.Value);
            }

            if (model.MaterialTechnicalValueId.HasValue)
            {
                query = query.Where(x => x.MaterialTechnicalValueId == model.MaterialTechnicalValueId.Value);
            }

            return query
                .Select(x => CreateModel(x))
                .ToList();
        }

        public EquipmentMovementHistoryViewModel? GetElement(EquipmentMovementHistorySearchModel model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = _context.EquipmentMovementHistories
                .Include(x => x.MaterialTechnicalValue)
                .FirstOrDefault(x =>
                    model.MaterialTechnicalValueId.HasValue && x.MaterialTechnicalValueId == model.MaterialTechnicalValueId.Value);

            return entity != null ? CreateModel(entity) : null;
        }

        public EquipmentMovementHistoryViewModel? Insert(EquipmentMovementHistoryBindingModel model)
        {
            var entity = new EquipmentMovementHistory
            {
                MoveDate = model.MoveDate,
                Reason = model.Reason,
                MaterialTechnicalValueId = model.MaterialTechnicalValueId
            };

            _context.EquipmentMovementHistories.Add(entity);
            _context.SaveChanges();

            return CreateModel(entity);
        }

        public EquipmentMovementHistoryViewModel? Update(EquipmentMovementHistoryBindingModel model)
        {
            var entity = _context.EquipmentMovementHistories.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null)
            {
                return null;
            }

            entity.MoveDate = model.MoveDate;
            entity.Reason = model.Reason;
            entity.MaterialTechnicalValueId = model.MaterialTechnicalValueId;

            _context.SaveChanges();

            return CreateModel(entity);
        }

        public EquipmentMovementHistoryViewModel? Delete(EquipmentMovementHistoryBindingModel model)
        {
            var entity = _context.EquipmentMovementHistories.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null)
            {
                return null;
            }

            var result = CreateModel(entity);
            _context.EquipmentMovementHistories.Remove(entity);
            _context.SaveChanges();

            return result;
        }

        private static EquipmentMovementHistoryViewModel CreateModel(EquipmentMovementHistory entity)
        {
            return new EquipmentMovementHistoryViewModel
            {
                Id = entity.Id,
                MoveDate = entity.MoveDate,
                Reason = entity.Reason,
                MaterialTechnicalValueId = entity.MaterialTechnicalValueId,
                MaterialTechnicalValueName = entity.MaterialTechnicalValue?.FullName ?? string.Empty
            };
        }
    }
}
