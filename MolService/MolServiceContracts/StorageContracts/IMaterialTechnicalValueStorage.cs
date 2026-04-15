using MolServiceContracts.BindingModels;
using MolServiceContracts.SearchModels;
using MolServiceContracts.ViewModels;
using MolServiceContracts.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.StorageContracts
{
    public interface IMaterialTechnicalValueStorage
    {
        List<MaterialTechnicalValueViewModel> GetFullList();

        List<MaterialTechnicalValueViewModel> GetFilteredList(MaterialTechnicalValueSearchModel model);

        MaterialTechnicalValueViewModel? GetElement(MaterialTechnicalValueSearchModel model);

        MaterialTechnicalValueViewModel? Insert(MaterialTechnicalValueBindingModel model);

        MaterialTechnicalValueViewModel? Update(MaterialTechnicalValueBindingModel model);

        MaterialTechnicalValueViewModel? Delete(MaterialTechnicalValueBindingModel model);
        List<InventoryReportItemViewModel> GetInventoryReportItems();
        List<InventoryReportItemViewModel> GetInventoryReportItemsByClassroomIds(List<int> classroomIds);
    }
}
