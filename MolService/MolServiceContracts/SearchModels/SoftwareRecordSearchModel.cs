using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.SearchModels
{
    public class SoftwareRecordSearchModel
    {
        public int? Id { get; set; }

        public int? MaterialTechnicalValueId { get; set; }

        public int? SoftwareId { get; set; }

        public string? ClaimNumber { get; set; }
    }
}
