using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.ViewModels
{
    public class MaterialTechnicalValueRecordViewModel
    {
        public int Id { get; set; }

        public int MaterialTechnicalValueGroupId { get; set; }

        public string MaterialTechnicalValueGroupName { get; set; } = string.Empty;

        public int MaterialTechnicalValueId { get; set; }

        public string MaterialTechnicalValueName { get; set; } = string.Empty;

        public string FieldName { get; set; } = string.Empty;

        public string FieldValue { get; set; } = string.Empty;

        public int Order { get; set; }
    }
}
