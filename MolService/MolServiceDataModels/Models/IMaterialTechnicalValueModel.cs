using MolServiceDataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDataModels.Models
{
    public interface IMaterialTechnicalValueModel : IId
    {
        string InventoryNumber { get; }
        int? ClassroomId { get; }
        string FullName { get; }
        decimal Quantity { get; }
        string Description { get; }
        string Location { get; }
        MaterialTechnicalValueSourceType SourceType { get; }
        string ExternalKey { get; }
        int MaterialResponsiblePersonId { get; }
    }
}
