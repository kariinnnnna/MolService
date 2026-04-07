using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MolServiceContracts.BusinessLogicContracts;

namespace MolServiceRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CoreImportController : ControllerBase
    {
        private readonly ICoreClassroomImportLogic _importLogic;

        public CoreImportController(ICoreClassroomImportLogic importLogic)
        {
            _importLogic = importLogic;
        }

        [HttpPost]
        public async Task<IActionResult> ImportClassrooms()
        {
            await _importLogic.ImportClassroomsAsync();
            return Ok("Синхронизация аудиторий из core завершена");
        }
    }
}
