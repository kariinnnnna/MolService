using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.SearchModels
{
    public class MaterialResponsiblePersonSearchModel
    {
        public int? Id { get; set; }

        public string? FullName { get; set; }

        public string? Position { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }
    }
}
