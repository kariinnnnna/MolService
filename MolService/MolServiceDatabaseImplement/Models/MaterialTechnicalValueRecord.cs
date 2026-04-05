using MolServiceDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDatabaseImplement.Models
{
    public class MaterialTechnicalValueRecord : IMaterialTechnicalValueRecordModel
    {
        public int Id { get; set; }

        public int MaterialTechnicalValueGroupId { get; set; }

        public int MaterialTechnicalValueId { get; set; }

        public string FieldName { get; set; } = string.Empty;

        public string FieldValue { get; set; } = string.Empty;

        public int Order { get; set; }

        public virtual MaterialTechnicalValueGroup? MaterialTechnicalValueGroup { get; set; }

        public virtual MaterialTechnicalValue? MaterialTechnicalValue { get; set; }
    }
}
