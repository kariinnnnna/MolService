using MolServiceBusinessLogic.Helpers;
using MolServiceContracts.BindingModels;
using MolServiceContracts.BusinessLogicContracts;
using MolServiceContracts.SearchModels;
using MolServiceContracts.StorageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            foreach (var coreClassroom in coreClassrooms)
            {
                var existing = _classroomStorage.GetElement(new ClassroomSearchModel
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
        }
    }
}
