using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDataModels.Models
{
    public interface ISoftwareRecordModel : IId
    {
        int MaterialTechnicalValueId { get; }
        int SoftwareId { get; }
        string SetupDescription { get; }
        string ClaimNumber { get; }
    }
}
