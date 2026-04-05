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
    public interface IMaterialTechnicalValueLogic
    {
        List<MaterialTechnicalValueViewModel>? ReadList(MaterialTechnicalValueSearchModel? model);

        MaterialTechnicalValueViewModel? ReadElement(MaterialTechnicalValueSearchModel model);

        MaterialTechnicalValueViewModel? Create(MaterialTechnicalValueBindingModel model);

        MaterialTechnicalValueViewModel? Update(MaterialTechnicalValueBindingModel model);

        bool Delete(MaterialTechnicalValueBindingModel model);
    }
}
