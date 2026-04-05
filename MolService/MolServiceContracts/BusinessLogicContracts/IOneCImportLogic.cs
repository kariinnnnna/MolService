using MolServiceContracts.BindingModels;
using MolServiceContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.BusinessLogicContracts
{
    public interface IOneCImportLogic
    {
        Task<OneCImportResultViewModel> ImportFromOneCAsync(OneCImportBindingModel model);
    }
}
