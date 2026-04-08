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
    public class SoftwareRecordStorage : ISoftwareRecordStorage
    {
        private readonly MOLServiceDatabase _context;

        public SoftwareRecordStorage(MOLServiceDatabase context)
        {
            _context = context;
        }

        public List<SoftwareRecordViewModel> GetFullList()
        {
            return _context.SoftwareRecords
                .Include(x => x.MaterialTechnicalValue)
                .Include(x => x.Software)
                .OrderBy(x => x.MaterialTechnicalValue!.FullName)
                .ThenBy(x => x.Software!.SoftwareName)
                .Select(x => CreateModel(x))
                .ToList();
        }

        public List<SoftwareRecordViewModel> GetFilteredList(SoftwareRecordSearchModel model)
        {
            if (model == null)
            {
                return new();
            }

            var query = _context.SoftwareRecords
                .Include(x => x.MaterialTechnicalValue)
                .Include(x => x.Software)
                .AsQueryable();

            if (model.Id.HasValue)
            {
                query = query.Where(x => x.Id == model.Id.Value);
            }

            if (model.MaterialTechnicalValueId.HasValue)
            {
                query = query.Where(x => x.MaterialTechnicalValueId == model.MaterialTechnicalValueId.Value);
            }

            if (model.SoftwareId.HasValue)
            {
                query = query.Where(x => x.SoftwareId == model.SoftwareId.Value);
            }

            if (!string.IsNullOrWhiteSpace(model.ClaimNumber))
            {
                query = query.Where(x => x.ClaimNumber.Contains(model.ClaimNumber));
            }

            return query
                .OrderBy(x => x.MaterialTechnicalValue!.FullName)
                .ThenBy(x => x.Software!.SoftwareName)
                .Select(x => CreateModel(x))
                .ToList();
        }

        public SoftwareRecordViewModel? GetElement(SoftwareRecordSearchModel model)
        {
            if (model == null)
            {
                return null;
            }

            var query = _context.SoftwareRecords
                .Include(x => x.MaterialTechnicalValue)
                .Include(x => x.Software)
                .AsQueryable();

            if (model.Id.HasValue)
            {
                query = query.Where(x => x.Id == model.Id.Value);
            }
            else if (model.MaterialTechnicalValueId.HasValue && model.SoftwareId.HasValue)
            {
                query = query.Where(x =>
                    x.MaterialTechnicalValueId == model.MaterialTechnicalValueId.Value &&
                    x.SoftwareId == model.SoftwareId.Value);
            }
            else
            {
                return null;
            }

            var entity = query.FirstOrDefault();
            return entity != null ? CreateModel(entity) : null;
        }

        public SoftwareRecordViewModel? Insert(SoftwareRecordBindingModel model)
        {
            var entity = new SoftwareRecord
            {
                MaterialTechnicalValueId = model.MaterialTechnicalValueId,
                SoftwareId = model.SoftwareId,
                SetupDescription = model.SetupDescription,
                ClaimNumber = model.ClaimNumber
            };

            _context.SoftwareRecords.Add(entity);
            _context.SaveChanges();

            entity = _context.SoftwareRecords
                .Include(x => x.MaterialTechnicalValue)
                .Include(x => x.Software)
                .First(x => x.Id == entity.Id);

            return CreateModel(entity);
        }

        public SoftwareRecordViewModel? Update(SoftwareRecordBindingModel model)
        {
            var entity = _context.SoftwareRecords
                .Include(x => x.MaterialTechnicalValue)
                .Include(x => x.Software)
                .FirstOrDefault(x => x.Id == model.Id);

            if (entity == null)
            {
                return null;
            }

            entity.MaterialTechnicalValueId = model.MaterialTechnicalValueId;
            entity.SoftwareId = model.SoftwareId;
            entity.SetupDescription = model.SetupDescription;
            entity.ClaimNumber = model.ClaimNumber;

            _context.SaveChanges();

            return CreateModel(entity);
        }

        public SoftwareRecordViewModel? Delete(SoftwareRecordBindingModel model)
        {
            var entity = _context.SoftwareRecords
                .Include(x => x.MaterialTechnicalValue)
                .Include(x => x.Software)
                .FirstOrDefault(x => x.Id == model.Id);

            if (entity == null)
            {
                return null;
            }

            var result = CreateModel(entity);

            _context.SoftwareRecords.Remove(entity);
            _context.SaveChanges();

            return result;
        }

        private static SoftwareRecordViewModel CreateModel(SoftwareRecord entity)
        {
            return new SoftwareRecordViewModel
            {
                Id = entity.Id,
                MaterialTechnicalValueId = entity.MaterialTechnicalValueId,
                MaterialTechnicalValueName = entity.MaterialTechnicalValue?.FullName ?? string.Empty,
                SoftwareId = entity.SoftwareId,
                SoftwareName = entity.Software?.SoftwareName ?? string.Empty,
                SetupDescription = entity.SetupDescription,
                ClaimNumber = entity.ClaimNumber
            };
        }
    }
}
