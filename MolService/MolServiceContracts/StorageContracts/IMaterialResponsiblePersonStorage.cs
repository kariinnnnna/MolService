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
    public interface IMaterialResponsiblePersonStorage
    {
        List<MaterialResponsiblePersonViewModel> GetFullList();

        List<MaterialResponsiblePersonViewModel> GetFilteredList(MaterialResponsiblePersonSearchModel model);

        MaterialResponsiblePersonViewModel? GetElement(MaterialResponsiblePersonSearchModel model);

        MaterialResponsiblePersonViewModel? Insert(MaterialResponsiblePersonBindingModel model);

        MaterialResponsiblePersonViewModel? Update(MaterialResponsiblePersonBindingModel model);

        MaterialResponsiblePersonViewModel? Delete(MaterialResponsiblePersonBindingModel model);
    }
}
