using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDataModels.Models
{
    public interface ISoftwareModel : IId
    {
        string SoftwareName { get; }
        string SoftwareDescription { get; }
        string SoftwareKey { get; }
        string SoftwareK { get; }
    }
}
