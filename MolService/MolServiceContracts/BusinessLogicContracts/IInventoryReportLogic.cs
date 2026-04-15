using MolServiceContracts.BindingModels;
using MolServiceContracts.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceContracts.BusinessLogicContracts
{
    public interface IInventoryReportLogic
    {
        FullInventoryReportViewModel GetFullInventoryReport();
        ClassroomsInventoryReportViewModel GetClassroomsInventoryReport(ClassroomsInventoryReportBindingModel model);
    }
}
