using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MolServiceContracts.BindingModels;
using MolServiceContracts.BusinessLogicContracts;

namespace MolServiceRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InventoryReportController : ControllerBase
    {
        private readonly IInventoryReportLogic _logic;

        public InventoryReportController(IInventoryReportLogic logic)
        {
            _logic = logic;
        }

        [HttpGet]
        public IActionResult GetFullInventoryReport()
        {
            return Ok(_logic.GetFullInventoryReport());
        }

        [HttpPost]
        public IActionResult GetClassroomsInventoryReport([FromBody] ClassroomsInventoryReportBindingModel model)
        {
            return Ok(_logic.GetClassroomsInventoryReport(model));
        }
    }
}
