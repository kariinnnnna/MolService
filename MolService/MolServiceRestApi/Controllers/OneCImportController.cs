using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MolServiceContracts.BindingModels;
using MolServiceContracts.BusinessLogicContracts;

namespace MolServiceRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OneCImportController : ControllerBase
    {
        private readonly IOneCImportLogic _logic;

        public OneCImportController(IOneCImportLogic logic)
        {
            _logic = logic;
        }

        [HttpPost]
        public async Task<IActionResult> ImportInventory([FromBody] OneCImportBindingModel model)
        {
            try
            {
                var result = await _logic.ImportFromOneCAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
