using Microsoft.EntityFrameworkCore;
using MolServiceContracts.BindingModels;
using MolServiceContracts.SearchModels;
using MolServiceContracts.StorageContracts;
using MolServiceContracts.ViewModels;
using MolServiceContracts.ViewModels.Reports;
using MolServiceDatabaseImplement.Models;

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
                .OrderBy(x => x.SourceType)
                .ThenBy(x => x.FullName)
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

            if (model.SourceType.HasValue)
            {
                query = query.Where(x => x.SourceType == model.SourceType.Value);
            }

            if (!string.IsNullOrWhiteSpace(model.ExternalKey))
            {
                var externalKey = model.ExternalKey.Trim();
                query = query.Where(x => x.ExternalKey == externalKey);
            }

            if (!string.IsNullOrWhiteSpace(model.InventoryNumber))
            {
                var inventoryNumber = model.InventoryNumber.Trim();
                query = query.Where(x => x.InventoryNumber.Contains(inventoryNumber));
            }

            if (model.ClassroomId.HasValue)
            {
                query = query.Where(x => x.ClassroomId == model.ClassroomId.Value);
            }

            if (!string.IsNullOrWhiteSpace(model.FullName))
            {
                var fullName = model.FullName.Trim();
                query = query.Where(x => x.FullName.Contains(fullName));
            }

            if (!string.IsNullOrWhiteSpace(model.Location))
            {
                var location = model.Location.Trim();
                query = query.Where(x => x.Location.Contains(location));
            }

            if (!string.IsNullOrWhiteSpace(model.SearchText))
            {
                var searchText = model.SearchText.Trim();

                query = query.Where(x =>
                    x.InventoryNumber.Contains(searchText) ||
                    x.FullName.Contains(searchText) ||
                    x.Description.Contains(searchText) ||
                    x.Location.Contains(searchText) ||
                    (x.MaterialResponsiblePerson != null &&
                     x.MaterialResponsiblePerson.FullName.Contains(searchText)) ||
                    (x.Classroom != null &&
                     x.Classroom.Number.Contains(searchText)));
            }

            query = query
                .OrderBy(x => x.SourceType)
                .ThenBy(x => x.FullName);

            if (model.Page > 0 && model.PageSize > 0)
            {
                query = query
                    .Skip((model.Page - 1) * model.PageSize)
                    .Take(model.PageSize);
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

            var query = _context.MaterialTechnicalValues
                .Include(x => x.Classroom)
                .Include(x => x.MaterialResponsiblePerson)
                .AsQueryable();

            if (model.Id.HasValue)
            {
                query = query.Where(x => x.Id == model.Id.Value);
            }
            else if (!string.IsNullOrWhiteSpace(model.ExternalKey))
            {
                var externalKey = model.ExternalKey.Trim();
                query = query.Where(x => x.ExternalKey == externalKey);
            }
            else if (!string.IsNullOrWhiteSpace(model.InventoryNumber))
            {
                var inventoryNumber = model.InventoryNumber.Trim();
                query = query.Where(x => x.InventoryNumber == inventoryNumber);

                if (model.SourceType.HasValue)
                {
                    query = query.Where(x => x.SourceType == model.SourceType.Value);
                }
            }
            else
            {
                return null;
            }

            var entity = query.FirstOrDefault();

            return entity != null ? CreateModel(entity) : null;
        }

        public MaterialTechnicalValueViewModel? Insert(MaterialTechnicalValueBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = new MaterialTechnicalValue
            {
                InventoryNumber = model.InventoryNumber,
                ClassroomId = model.ClassroomId,
                FullName = model.FullName,
                Quantity = model.Quantity,
                Description = model.Description,
                Location = model.Location,
                MaterialResponsiblePersonId = model.MaterialResponsiblePersonId,

                SourceType = model.SourceType,
                ExternalKey = model.ExternalKey ?? string.Empty
            };

            _context.MaterialTechnicalValues.Add(entity);
            _context.SaveChanges();

            return CreateModel(entity);
        }

        public MaterialTechnicalValueViewModel? Update(MaterialTechnicalValueBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = _context.MaterialTechnicalValues
                .FirstOrDefault(x => x.Id == model.Id);

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

            entity.SourceType = model.SourceType;
            entity.ExternalKey = model.ExternalKey ?? string.Empty;

            _context.SaveChanges();

            return CreateModel(entity);
        }

        public MaterialTechnicalValueViewModel? Delete(MaterialTechnicalValueBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = _context.MaterialTechnicalValues
                .Include(x => x.Classroom)
                .Include(x => x.MaterialResponsiblePerson)
                .FirstOrDefault(x => x.Id == model.Id);

            if (entity == null)
            {
                return null;
            }

            var result = CreateModel(entity);

            _context.MaterialTechnicalValues.Remove(entity);
            _context.SaveChanges();

            return result;
        }

        public List<InventoryReportItemViewModel> GetInventoryReportItems()
        {
            return _context.MaterialTechnicalValues
                .Include(x => x.Classroom)
                .Include(x => x.MaterialResponsiblePerson)
                .Select(x => new InventoryReportItemViewModel
                {
                    InventoryNumber = x.InventoryNumber,
                    FullName = x.FullName,
                    Quantity = x.Quantity,
                    Description = x.Description,
                    Location = x.Location,
                    ClassroomId = x.ClassroomId,
                    ClassroomNumber = x.Classroom != null
                        ? x.Classroom.Number
                        : string.Empty,
                    MaterialResponsiblePersonId = x.MaterialResponsiblePersonId,
                    MaterialResponsiblePersonName = x.MaterialResponsiblePerson != null
                        ? x.MaterialResponsiblePerson.FullName
                        : string.Empty
                })
                .OrderBy(x => x.ClassroomNumber)
                .ThenBy(x => x.FullName)
                .ToList();
        }

        public List<InventoryReportItemViewModel> GetInventoryReportItemsByClassroomIds(List<int> classroomIds)
        {
            if (classroomIds == null || classroomIds.Count == 0)
            {
                return new();
            }

            return _context.MaterialTechnicalValues
                .Include(x => x.Classroom)
                .Include(x => x.MaterialResponsiblePerson)
                .Where(x => x.ClassroomId.HasValue &&
                            classroomIds.Contains(x.ClassroomId.Value))
                .Select(x => new InventoryReportItemViewModel
                {
                    InventoryNumber = x.InventoryNumber,
                    FullName = x.FullName,
                    Quantity = x.Quantity,
                    Description = x.Description,
                    Location = x.Location,
                    ClassroomId = x.ClassroomId,
                    ClassroomNumber = x.Classroom != null
                        ? x.Classroom.Number
                        : string.Empty,
                    MaterialResponsiblePersonId = x.MaterialResponsiblePersonId,
                    MaterialResponsiblePersonName = x.MaterialResponsiblePerson != null
                        ? x.MaterialResponsiblePerson.FullName
                        : string.Empty
                })
                .OrderBy(x => x.ClassroomNumber)
                .ThenBy(x => x.FullName)
                .ToList();
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
                MaterialResponsiblePersonName = entity.MaterialResponsiblePerson?.FullName ?? string.Empty,

                SourceType = entity.SourceType,
                ExternalKey = entity.ExternalKey
            };
        }
        public PagedResultViewModel<MaterialTechnicalValueViewModel> GetPagedList(
    MaterialTechnicalValueSearchModel model)
        {
            model ??= new MaterialTechnicalValueSearchModel();

            var page = model.Page <= 0 ? 1 : model.Page;
            var pageSize = model.PageSize <= 0 ? 20 : model.PageSize;

            var query = _context.MaterialTechnicalValues
                .Include(x => x.Classroom)
                .Include(x => x.MaterialResponsiblePerson)
                .AsQueryable();

            if (model.Id.HasValue)
            {
                query = query.Where(x => x.Id == model.Id.Value);
            }

            if (model.SourceType.HasValue)
            {
                query = query.Where(x => x.SourceType == model.SourceType.Value);
            }

            if (!string.IsNullOrWhiteSpace(model.ExternalKey))
            {
                var externalKey = model.ExternalKey.Trim();
                query = query.Where(x => x.ExternalKey == externalKey);
            }

            if (!string.IsNullOrWhiteSpace(model.InventoryNumber))
            {
                var inventoryNumber = model.InventoryNumber.Trim();
                query = query.Where(x => x.InventoryNumber.Contains(inventoryNumber));
            }

            if (model.ClassroomId.HasValue)
            {
                query = query.Where(x => x.ClassroomId == model.ClassroomId.Value);
            }

            if (!string.IsNullOrWhiteSpace(model.FullName))
            {
                var fullName = model.FullName.Trim();
                query = query.Where(x => x.FullName.Contains(fullName));
            }

            if (!string.IsNullOrWhiteSpace(model.Location))
            {
                var location = model.Location.Trim();
                query = query.Where(x => x.Location.Contains(location));
            }

            if (!string.IsNullOrWhiteSpace(model.SearchText))
            {
                var searchText = model.SearchText.Trim();

                query = query.Where(x =>
                    x.InventoryNumber.Contains(searchText) ||
                    x.FullName.Contains(searchText) ||
                    x.Description.Contains(searchText) ||
                    x.Location.Contains(searchText) ||
                    (x.MaterialResponsiblePerson != null &&
                     x.MaterialResponsiblePerson.FullName.Contains(searchText)) ||
                    (x.Classroom != null &&
                     x.Classroom.Number.Contains(searchText)));
            }

            var totalCount = query.Count();

            var items = query
                .OrderBy(x => x.SourceType)
                .ThenBy(x => x.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => CreateModel(x))
                .ToList();

            return new PagedResultViewModel<MaterialTechnicalValueViewModel>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}