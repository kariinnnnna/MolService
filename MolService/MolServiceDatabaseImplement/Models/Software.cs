using MolServiceDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDatabaseImplement.Models
{
    public class Software : ISoftwareModel
    {
        public int Id { get; set; }

        public string SoftwareName { get; set; } = string.Empty;

        public string SoftwareDescription { get; set; } = string.Empty;

        public string SoftwareKey { get; set; } = string.Empty;

        public string SoftwareK { get; set; } = string.Empty;

        public virtual List<SoftwareRecord> SoftwareRecords { get; set; } = new();
    }
}
