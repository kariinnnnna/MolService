using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.ViewModels
{
    public class SoftwareRecordViewModel
    {
        public int Id { get; set; }

        public int MaterialTechnicalValueId { get; set; }

        public string MaterialTechnicalValueName { get; set; } = string.Empty;

        public int SoftwareId { get; set; }

        public string SoftwareName { get; set; } = string.Empty;

        public string SetupDescription { get; set; } = string.Empty;

        public string ClaimNumber { get; set; } = string.Empty;
    }
}
