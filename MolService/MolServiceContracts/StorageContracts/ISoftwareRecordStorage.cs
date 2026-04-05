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
    public interface ISoftwareRecordStorage
    {
        List<SoftwareRecordViewModel> GetFullList();

        List<SoftwareRecordViewModel> GetFilteredList(SoftwareRecordSearchModel model);

        SoftwareRecordViewModel? GetElement(SoftwareRecordSearchModel model);

        SoftwareRecordViewModel? Insert(SoftwareRecordBindingModel model);

        SoftwareRecordViewModel? Update(SoftwareRecordBindingModel model);

        SoftwareRecordViewModel? Delete(SoftwareRecordBindingModel model);
    }
}
