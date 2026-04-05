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
    public interface IMaterialTechnicalValueGroupLogic
    {
        List<MaterialTechnicalValueGroupViewModel>? ReadList(MaterialTechnicalValueGroupSearchModel? model);

        MaterialTechnicalValueGroupViewModel? ReadElement(MaterialTechnicalValueGroupSearchModel model);

        MaterialTechnicalValueGroupViewModel? Create(MaterialTechnicalValueGroupBindingModel model);

        MaterialTechnicalValueGroupViewModel? Update(MaterialTechnicalValueGroupBindingModel model);

        bool Delete(MaterialTechnicalValueGroupBindingModel model);
    }
}
