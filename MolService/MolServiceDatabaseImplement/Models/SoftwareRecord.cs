using MolServiceDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDatabaseImplement.Models
{
    public class SoftwareRecord : ISoftwareRecordModel
    {
        public int Id { get; set; }

        public int MaterialTechnicalValueId { get; set; }

        public int SoftwareId { get; set; }

        public string SetupDescription { get; set; } = string.Empty;

        public string ClaimNumber { get; set; } = string.Empty;

        public virtual MaterialTechnicalValue? MaterialTechnicalValue { get; set; }

        public virtual Software? Software { get; set; }
    }
}
