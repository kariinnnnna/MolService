using MolServiceDataModels.Enums;
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
        public MaterialTechnicalValueSourceType? SourceType { get; set; }

        public string? ExternalKey { get; set; }

        public string? SearchText { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;

        public int? MaterialResponsiblePersonId { get; set; }
    }
}
