using MolServiceContracts.BindingModels;
using MolServiceContracts.SearchModels;
using MolServiceContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.BusinessLogicContracts
{
    public interface ISoftwareRecordLogic
    {
        List<SoftwareRecordViewModel>? ReadList(SoftwareRecordSearchModel? model);

        SoftwareRecordViewModel? ReadElement(SoftwareRecordSearchModel model);

        SoftwareRecordViewModel? Create(SoftwareRecordBindingModel model);

        SoftwareRecordViewModel? Update(SoftwareRecordBindingModel model);

        bool Delete(SoftwareRecordBindingModel model);

        SoftwareAssignToClassroomResultViewModel AssignToClassroom(SoftwareAssignToClassroomBindingModel model);

    }
}
