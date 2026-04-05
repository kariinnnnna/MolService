using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.SearchModels
{
    public class SoftwareSearchModel
    {
        public int? Id { get; set; }

        public string? SoftwareName { get; set; }

        public string? SoftwareKey { get; set; }
    }
}
