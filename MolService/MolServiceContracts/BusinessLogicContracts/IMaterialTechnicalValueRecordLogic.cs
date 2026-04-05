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
    public interface IMaterialTechnicalValueRecordLogic
    {
        List<MaterialTechnicalValueRecordViewModel>? ReadList(MaterialTechnicalValueRecordSearchModel? model);

        MaterialTechnicalValueRecordViewModel? ReadElement(MaterialTechnicalValueRecordSearchModel model);

        MaterialTechnicalValueRecordViewModel? Create(MaterialTechnicalValueRecordBindingModel model);

        MaterialTechnicalValueRecordViewModel? Update(MaterialTechnicalValueRecordBindingModel model);

        bool Delete(MaterialTechnicalValueRecordBindingModel model);
    }
}
