using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.ViewModels.Reports
{
    public class InventoryReportItemViewModel
    {
        public string InventoryNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        public int? ClassroomId { get; set; }
        public string ClassroomNumber { get; set; } = string.Empty;

        public int? MaterialResponsiblePersonId { get; set; }
        public string MaterialResponsiblePersonName { get; set; } = string.Empty;
    }
}
