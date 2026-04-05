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
    public interface ISoftwareLogic
    {
        List<SoftwareViewModel>? ReadList(SoftwareSearchModel? model);

        SoftwareViewModel? ReadElement(SoftwareSearchModel model);

        SoftwareViewModel? Create(SoftwareBindingModel model);

        SoftwareViewModel? Update(SoftwareBindingModel model);

        bool Delete(SoftwareBindingModel model);
    }
}
