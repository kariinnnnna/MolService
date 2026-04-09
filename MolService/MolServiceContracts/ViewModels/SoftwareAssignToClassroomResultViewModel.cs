using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.ViewModels
{
    public class SoftwareAssignToClassroomResultViewModel
    {
        public int ClassroomId { get; set; }
        public int SoftwareId { get; set; }

        public int FoundEquipmentCount { get; set; }
        public int CreatedCount { get; set; }
        public int SkippedDuplicatesCount { get; set; }

        public List<string> Errors { get; set; } = new();
        public bool HasErrors => Errors.Count > 0;
    }
}
