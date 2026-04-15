using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.BindingModels
{
    public class ClassroomsInventoryReportBindingModel
    {
        public List<int> ClassroomIds { get; set; } = new();
    }
}
