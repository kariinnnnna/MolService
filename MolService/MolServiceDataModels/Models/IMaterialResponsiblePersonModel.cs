using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDataModels.Models
{
    public interface IMaterialResponsiblePersonModel : IId
    {
        string FullName { get; }
        string Position { get; }
        string Phone { get; }
        string Email { get; }
    }
}
