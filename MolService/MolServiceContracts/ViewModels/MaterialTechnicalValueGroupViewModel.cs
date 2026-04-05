using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.ViewModels
{
    public class MaterialTechnicalValueGroupViewModel
    {
        public int Id { get; set; }

        public string GroupName { get; set; } = string.Empty;

        public int Order { get; set; }
    }
}
