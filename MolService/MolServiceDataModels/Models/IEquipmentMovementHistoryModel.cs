using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDataModels.Models
{
    public interface IEquipmentMovementHistoryModel : IId
    {
        DateTime MoveDate { get; }
        string Reason { get; }
        decimal Quantity { get; }
        string Comment { get; }
        int MaterialTechnicalValueId { get; }
    }
}
