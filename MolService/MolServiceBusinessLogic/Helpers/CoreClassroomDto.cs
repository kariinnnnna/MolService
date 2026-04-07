using MolServiceDataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceBusinessLogic.Helpers
{
    public class CoreClassroomDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public ClassroomType Type { get; set; }
        public int Capacity { get; set; }
        public bool NotUseInSchedule { get; set; }
        public bool HasProjector { get; set; }
    }
}
