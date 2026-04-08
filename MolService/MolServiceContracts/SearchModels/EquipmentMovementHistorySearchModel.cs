using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.SearchModels
{
    public class EquipmentMovementHistorySearchModel
    {
        public int? Id { get; set; }

        public DateTime? MoveDateFrom { get; set; }

        public DateTime? MoveDateTo { get; set; }

        public int? MaterialTechnicalValueId { get; set; }

        public string? ReasonContains { get; set; }
    }
}
