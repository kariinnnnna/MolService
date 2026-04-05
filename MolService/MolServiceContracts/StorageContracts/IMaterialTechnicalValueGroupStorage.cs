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
    public interface IMaterialTechnicalValueGroupStorage
    {
        List<MaterialTechnicalValueGroupViewModel> GetFullList();

        List<MaterialTechnicalValueGroupViewModel> GetFilteredList(MaterialTechnicalValueGroupSearchModel model);

        MaterialTechnicalValueGroupViewModel? GetElement(MaterialTechnicalValueGroupSearchModel model);

        MaterialTechnicalValueGroupViewModel? Insert(MaterialTechnicalValueGroupBindingModel model);

        MaterialTechnicalValueGroupViewModel? Update(MaterialTechnicalValueGroupBindingModel model);

        MaterialTechnicalValueGroupViewModel? Delete(MaterialTechnicalValueGroupBindingModel model);
    }
}
