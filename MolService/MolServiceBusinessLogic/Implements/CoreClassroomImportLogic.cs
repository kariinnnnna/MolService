using MolServiceBusinessLogic.Helpers;
using MolServiceContracts.BindingModels;
using MolServiceContracts.BusinessLogicContracts;
using MolServiceContracts.SearchModels;
using MolServiceContracts.StorageContracts;

namespace MolServiceBusinessLogic.Implements
{
    public class CoreClassroomImportLogic : ICoreClassroomImportLogic
    {
        private readonly IClassroomStorage _classroomStorage;
        private readonly CoreApiService _coreApiService;

        public CoreClassroomImportLogic(
            IClassroomStorage classroomStorage,
            CoreApiService coreApiService)
        {
            _classroomStorage = classroomStorage;
            _coreApiService = coreApiService;
        }

        public async Task ImportClassroomsAsync()
        {
            var coreClassrooms = await _coreApiService.GetClassroomsAsync();

            var actualCoreIds = coreClassrooms
                .Select(x => x.Id)
                .ToHashSet();

            foreach (var coreClassroom in coreClassrooms)
            {
                var existing = _classroomStorage.GetElement(
                    new ClassroomSearchModel
                    {
                        CoreSystemId = coreClassroom.Id
                    });

                var model = new ClassroomBindingModel
                {
                    CoreSystemId = coreClassroom.Id,
                    Number = coreClassroom.Number,
                    Type = coreClassroom.Type,
                    Capacity = coreClassroom.Capacity,
                    NotUseInSchedule = coreClassroom.NotUseInSchedule,
                    HasProjector = coreClassroom.HasProjector
                };

                if (existing == null)
                {
                    _classroomStorage.Insert(model);
                }
                else
                {
                    model.Id = existing.Id;
                    _classroomStorage.Update(model);
                }
            }

            var localCoreClassrooms = _classroomStorage.GetFullList()
                .Where(x => x.CoreSystemId > 0)
                .ToList();

            var deletedFromCoreClassrooms = localCoreClassrooms
                .Where(x => !actualCoreIds.Contains(x.CoreSystemId))
                .ToList();

            foreach (var classroom in deletedFromCoreClassrooms)
            {
                _classroomStorage.Delete(new ClassroomBindingModel
                {
                    Id = classroom.Id
                });
            }
        }
    }
}