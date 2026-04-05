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
    public interface IMaterialResponsiblePersonLogic
    {
        List<MaterialResponsiblePersonViewModel>? ReadList(MaterialResponsiblePersonSearchModel? model);

        MaterialResponsiblePersonViewModel? ReadElement(MaterialResponsiblePersonSearchModel model);

        MaterialResponsiblePersonViewModel? Create(MaterialResponsiblePersonBindingModel model);

        MaterialResponsiblePersonViewModel? Update(MaterialResponsiblePersonBindingModel model);

        bool Delete(MaterialResponsiblePersonBindingModel model);
    }
}
