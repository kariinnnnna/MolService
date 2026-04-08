using MolServiceDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDatabaseImplement.Models
{
    public class EquipmentMovementHistory : IEquipmentMovementHistoryModel
    {
        public int Id { get; set; }

        public DateTime MoveDate { get; set; }

        public string Reason { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public string Comment { get; set; } = string.Empty;

        public int MaterialTechnicalValueId { get; set; }

        public virtual MaterialTechnicalValue? MaterialTechnicalValue { get; set; }
    }
}
