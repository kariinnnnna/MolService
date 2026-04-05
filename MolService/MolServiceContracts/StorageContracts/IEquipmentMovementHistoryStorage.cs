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
    public interface IEquipmentMovementHistoryStorage
    {
        List<EquipmentMovementHistoryViewModel> GetFullList();

        List<EquipmentMovementHistoryViewModel> GetFilteredList(EquipmentMovementHistorySearchModel model);

        EquipmentMovementHistoryViewModel? GetElement(EquipmentMovementHistorySearchModel model);

        EquipmentMovementHistoryViewModel? Insert(EquipmentMovementHistoryBindingModel model);

        EquipmentMovementHistoryViewModel? Update(EquipmentMovementHistoryBindingModel model);

        EquipmentMovementHistoryViewModel? Delete(EquipmentMovementHistoryBindingModel model);
    }
}
