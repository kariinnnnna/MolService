using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDataModels.Models
{
    public interface IMaterialTechnicalValueRecordModel : IId
    {
        int MaterialTechnicalValueGroupId { get; }
        int MaterialTechnicalValueId { get; }
        string FieldName { get; }
        string FieldValue { get; }
        int Order { get; }
    }
}
