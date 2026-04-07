using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.BusinessLogicContracts
{
    public interface ICoreClassroomImportLogic
    {
        Task ImportClassroomsAsync();
    }
}
