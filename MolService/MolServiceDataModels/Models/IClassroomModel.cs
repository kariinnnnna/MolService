using MolServiceDataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDataModels.Models
{
    public interface IClassroomModel : IId
    {
        int CoreSystemId { get; }
        string Number { get; }
        ClassroomType Type { get; }
        int Capacity { get; }
        bool NotUseInSchedule { get; }
        bool HasProjector { get; }
    }
}
