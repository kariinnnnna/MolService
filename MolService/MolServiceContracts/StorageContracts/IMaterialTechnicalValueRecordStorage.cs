using MolServiceContracts.BindingModels;
using MolServiceContracts.SearchModels;
using MolServiceContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.StorageContracts
{
    public interface IMaterialTechnicalValueRecordStorage
    {
        List<MaterialTechnicalValueRecordViewModel> GetFullList();

        List<MaterialTechnicalValueRecordViewModel> GetFilteredList(MaterialTechnicalValueRecordSearchModel model);

        MaterialTechnicalValueRecordViewModel? GetElement(MaterialTechnicalValueRecordSearchModel model);

        MaterialTechnicalValueRecordViewModel? Insert(MaterialTechnicalValueRecordBindingModel model);

        MaterialTechnicalValueRecordViewModel? Update(MaterialTechnicalValueRecordBindingModel model);

        MaterialTechnicalValueRecordViewModel? Delete(MaterialTechnicalValueRecordBindingModel model);
    }
}
