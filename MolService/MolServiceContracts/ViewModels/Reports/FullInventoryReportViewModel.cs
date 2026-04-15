using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.ViewModels.Reports
{
    public class FullInventoryReportViewModel
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<InventoryReportItemViewModel> Items { get; set; } = new();
    }
}
