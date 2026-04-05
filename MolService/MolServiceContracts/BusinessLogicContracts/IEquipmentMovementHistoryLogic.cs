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
    public interface IEquipmentMovementHistoryLogic
    {
        List<EquipmentMovementHistoryViewModel>? ReadList(EquipmentMovementHistorySearchModel? model);

        EquipmentMovementHistoryViewModel? ReadElement(EquipmentMovementHistorySearchModel model);

        EquipmentMovementHistoryViewModel? Create(EquipmentMovementHistoryBindingModel model);

        EquipmentMovementHistoryViewModel? Update(EquipmentMovementHistoryBindingModel model);

        bool Delete(EquipmentMovementHistoryBindingModel model);
    }
}
