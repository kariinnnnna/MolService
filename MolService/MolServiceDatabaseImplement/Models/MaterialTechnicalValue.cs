using MolServiceDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDatabaseImplement.Models
{
    public class MaterialTechnicalValue : IMaterialTechnicalValueModel
    {
        public int Id { get; set; }

        public string InventoryNumber { get; set; } = string.Empty;

        public int? ClassroomId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;


        public int MaterialResponsiblePersonId { get; set; }

        public virtual Classroom? Classroom { get; set; }

        public virtual MaterialResponsiblePerson? MaterialResponsiblePerson { get; set; }

        public virtual List<SoftwareRecord> SoftwareRecords { get; set; } = new();

        public virtual List<EquipmentMovementHistory> EquipmentMovementHistories { get; set; } = new();

        public virtual List<MaterialTechnicalValueRecord> MaterialTechnicalValueRecords { get; set; } = new();
    }
}
