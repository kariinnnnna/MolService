using MolServiceDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDatabaseImplement.Models
{
    public class MaterialTechnicalValueGroup : IMaterialTechnicalValueGroupModel
    {
        public int Id { get; set; }

        public string GroupName { get; set; } = string.Empty;

        public int Order { get; set; }

        public virtual List<MaterialTechnicalValueRecord> MaterialTechnicalValueRecords { get; set; } = new();
    }
}
