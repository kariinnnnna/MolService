using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.ViewModels.Reports
{
    public class ClassroomInventorySectionViewModel
    {
        public int ClassroomId { get; set; }
        public string ClassroomNumber { get; set; } = string.Empty;
        public List<InventoryReportItemViewModel> Items { get; set; } = new();
    }
}
