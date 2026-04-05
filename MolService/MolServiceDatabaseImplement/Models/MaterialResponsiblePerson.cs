using MolServiceDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDatabaseImplement.Models
{
    public class MaterialResponsiblePerson : IMaterialResponsiblePersonModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public virtual List<MaterialTechnicalValue> MaterialTechnicalValues { get; set; } = new();
    }
}
