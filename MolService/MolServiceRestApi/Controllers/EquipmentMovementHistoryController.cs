using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MolServiceContracts.BindingModels;
using MolServiceContracts.BusinessLogicContracts;
using MolServiceContracts.SearchModels;

namespace MolServiceRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EquipmentMovementHistoryController : ControllerBase
    {
        private readonly IEquipmentMovementHistoryLogic _logic;

        public EquipmentMovementHistoryController(IEquipmentMovementHistoryLogic logic)
        {
            _logic = logic;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_logic.ReadList(null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{materialTechnicalValueId}")]
        public IActionResult GetByMaterialTechnicalValue(int materialTechnicalValueId)
        {
            try
            {
                var result = _logic.ReadList(new EquipmentMovementHistorySearchModel
                {
                    MaterialTechnicalValueId = materialTechnicalValueId
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetFiltered([FromBody] EquipmentMovementHistorySearchModel model)
        {
            try
            {
                return Ok(_logic.ReadList(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetElement([FromBody] EquipmentMovementHistorySearchModel model)
        {
            try
            {
                var result = _logic.ReadElement(model);
                return result == null
                    ? NotFound("Запись списания оборудования не найдена")
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] EquipmentMovementHistoryBindingModel model)
        {
            try
            {
                var result = _logic.Create(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                var inner = ex.InnerException;

                while (inner != null)
                {
                    message += " | INNER: " + inner.Message;
                    inner = inner.InnerException;
                }

                return BadRequest(message);
            }
        }

        [HttpPost]
        public IActionResult Update([FromBody] EquipmentMovementHistoryBindingModel model)
        {
            try
            {
                var result = _logic.Update(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Delete([FromBody] EquipmentMovementHistoryBindingModel model)
        {
            try
            {
                return Ok(_logic.Delete(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
