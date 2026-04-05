using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDataModels.Models
{
    public interface IMaterialTechnicalValueGroupModel : IId
    {
        string GroupName { get; }
        int Order { get; }
    }
}
