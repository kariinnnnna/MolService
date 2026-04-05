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
    public interface ISoftwareStorage
    {
        List<SoftwareViewModel> GetFullList();

        List<SoftwareViewModel> GetFilteredList(SoftwareSearchModel model);

        SoftwareViewModel? GetElement(SoftwareSearchModel model);

        SoftwareViewModel? Insert(SoftwareBindingModel model);

        SoftwareViewModel? Update(SoftwareBindingModel model);

        SoftwareViewModel? Delete(SoftwareBindingModel model);
    }
}
