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
        private readonly IMaterialTechnicalValueStorage _materialTechnicalValueStorage;

        public SoftwareRecordLogic(
            ISoftwareRecordStorage storage,
            IMaterialTechnicalValueStorage materialTechnicalValueStorage)
        {
            _storage = storage;
            _materialTechnicalValueStorage = materialTechnicalValueStorage;
        }

        public List<SoftwareRecordViewModel>? ReadList(SoftwareRecordSearchModel? model)
        {
            return model == null ? _storage.GetFullList() : _storage.GetFilteredList(model);
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

            if (model.MaterialTechnicalValueId <= 0)
            {
                throw new ArgumentException("Не указано оборудование");
            }

            if (model.SoftwareId <= 0)
            {
                throw new ArgumentException("Не указано программное обеспечение");
            }

            var duplicate = _storage.GetElement(new SoftwareRecordSearchModel
            {
                MaterialTechnicalValueId = model.MaterialTechnicalValueId,
                SoftwareId = model.SoftwareId
            });

            if (duplicate != null)
            {
                throw new InvalidOperationException("Это программное обеспечение уже привязано к данному оборудованию");
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

            if (model.MaterialTechnicalValueId <= 0)
            {
                throw new ArgumentException("Не указано оборудование");
            }

            if (model.SoftwareId <= 0)
            {
                throw new ArgumentException("Не указано программное обеспечение");
            }

            var current = _storage.GetElement(new SoftwareRecordSearchModel
            {
                Id = model.Id
            });

            if (current == null)
            {
                throw new InvalidOperationException("Запись о привязке ПО не найдена");
            }

            var duplicate = _storage.GetElement(new SoftwareRecordSearchModel
            {
                MaterialTechnicalValueId = model.MaterialTechnicalValueId,
                SoftwareId = model.SoftwareId
            });

            if (duplicate != null && duplicate.Id != model.Id)
            {
                throw new InvalidOperationException("Это программное обеспечение уже привязано к данному оборудованию");
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
                throw new InvalidOperationException("Запись о привязке ПО не найдена");
            }

            return _storage.Delete(model) != null;
        }

        public SoftwareAssignToClassroomResultViewModel AssignToClassroom(SoftwareAssignToClassroomBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.ClassroomId <= 0)
            {
                throw new ArgumentException("Не указана аудитория");
            }

            if (model.SoftwareId <= 0)
            {
                throw new ArgumentException("Не указано программное обеспечение");
            }

            var equipmentList = _materialTechnicalValueStorage.GetFilteredList(
                new MaterialTechnicalValueSearchModel
                {
                    ClassroomId = model.ClassroomId
                }) ?? new List<MaterialTechnicalValueViewModel>();

            if (!equipmentList.Any())
            {
                throw new InvalidOperationException("В выбранной аудитории нет оборудования");
            }

            var result = new SoftwareAssignToClassroomResultViewModel
            {
                ClassroomId = model.ClassroomId,
                SoftwareId = model.SoftwareId,
                FoundEquipmentCount = equipmentList.Count
            };

            foreach (var equipment in equipmentList)
            {
                try
                {
                    var duplicate = _storage.GetElement(new SoftwareRecordSearchModel
                    {
                        MaterialTechnicalValueId = equipment.Id,
                        SoftwareId = model.SoftwareId
                    });

                    if (duplicate != null)
                    {
                        result.SkippedDuplicatesCount++;
                        continue;
                    }

                    _storage.Insert(new SoftwareRecordBindingModel
                    {
                        MaterialTechnicalValueId = equipment.Id,
                        SoftwareId = model.SoftwareId,
                        SetupDescription = model.SetupDescription?.Trim() ?? string.Empty,
                        ClaimNumber = model.ClaimNumber?.Trim() ?? string.Empty
                    });

                    result.CreatedCount++;
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"{equipment.FullName} ({equipment.InventoryNumber}): {ex.Message}");
                }
            }

            return result;
        }
    }
}
