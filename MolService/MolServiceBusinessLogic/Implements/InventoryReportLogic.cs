using MolServiceContracts.BindingModels;
using MolServiceContracts.BusinessLogicContracts;
using MolServiceContracts.StorageContracts;
using MolServiceContracts.ViewModels.Reports;

namespace MolServiceBusinessLogic.Implements
{
    public class InventoryReportLogic : IInventoryReportLogic
    {
        private readonly IMaterialTechnicalValueStorage _materialTechnicalValueStorage;
        private readonly IClassroomStorage _classroomStorage;

        public InventoryReportLogic(
            IMaterialTechnicalValueStorage materialTechnicalValueStorage,
            IClassroomStorage classroomStorage)
        {
            _materialTechnicalValueStorage = materialTechnicalValueStorage;
            _classroomStorage = classroomStorage;
        }

        public FullInventoryReportViewModel GetFullInventoryReport()
        {
            var items = _materialTechnicalValueStorage.GetInventoryReportItems()
                .OrderBy(x => string.IsNullOrWhiteSpace(x.ClassroomNumber) ? 1 : 0)
                .ThenBy(x => x.ClassroomNumber)
                .ThenBy(x => x.FullName)
                .ToList();

            return new FullInventoryReportViewModel
            {
                CreatedAt = DateTime.Now,
                Items = items
            };
        }

        public ClassroomsInventoryReportViewModel GetClassroomsInventoryReport(ClassroomsInventoryReportBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.ClassroomIds == null || model.ClassroomIds.Count == 0)
            {
                throw new ArgumentException("Не выбраны аудитории");
            }

            var classrooms = _classroomStorage.GetFilteredList(new MolServiceContracts.SearchModels.ClassroomSearchModel())
                ?.Where(x => model.ClassroomIds.Contains(x.Id))
                .OrderBy(x => x.Number)
                .ToList()
                ?? new();

            var items = _materialTechnicalValueStorage.GetInventoryReportItemsByClassroomIds(model.ClassroomIds);

            var result = new ClassroomsInventoryReportViewModel
            {
                CreatedAt = DateTime.Now
            };

            foreach (var classroom in classrooms)
            {
                result.Classrooms.Add(new ClassroomInventorySectionViewModel
                {
                    ClassroomId = classroom.Id,
                    ClassroomNumber = classroom.Number,
                    Items = items
                        .Where(x => x.ClassroomId == classroom.Id)
                        .OrderBy(x => x.FullName)
                        .ToList()
                });
            }

            return result;
        }
    }
}