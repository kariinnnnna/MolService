using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.SearchModels
{
    public class MaterialTechnicalValueRecordSearchModel
    {
        public int? Id { get; set; }

        public int? MaterialTechnicalValueGroupId { get; set; }

        public int? MaterialTechnicalValueId { get; set; }

        public string? FieldName { get; set; }
    }
}
