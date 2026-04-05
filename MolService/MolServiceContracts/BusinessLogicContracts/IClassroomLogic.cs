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
    public interface IClassroomLogic
    {
        List<ClassroomViewModel>? ReadList(ClassroomSearchModel? model);

        ClassroomViewModel? ReadElement(ClassroomSearchModel model);

        ClassroomViewModel? Create(ClassroomBindingModel model);

        ClassroomViewModel? Update(ClassroomBindingModel model);

        bool Delete(ClassroomBindingModel model);
    }
}
