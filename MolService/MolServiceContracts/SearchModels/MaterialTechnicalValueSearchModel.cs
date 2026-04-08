using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.SearchModels
{
    public class MaterialTechnicalValueSearchModel
    {
        public int? Id { get; set; }

        public string? InventoryNumber { get; set; }

        public int? ClassroomId { get; set; }

        public string? FullName { get; set; }

        public string? Location { get; set; }

        public int? MaterialResponsiblePersonId { get; set; }
    }
}
