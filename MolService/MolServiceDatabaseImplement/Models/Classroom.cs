using MolServiceDataModels.Enums;
using MolServiceDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDatabaseImplement.Models
{
    public class Classroom : IClassroomModel
    {
        public int Id { get; set; }

        public int CoreSystemId { get; set; }

        public string Number { get; set; } = string.Empty;

        public ClassroomType Type { get; set; }

        public int Capacity { get; set; }

        public bool NotUseInSchedule { get; set; }

        public bool HasProjector { get; set; }

        public virtual List<MaterialTechnicalValue> MaterialTechnicalValues { get; set; } = new();
    }
}
