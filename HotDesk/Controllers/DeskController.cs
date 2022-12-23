using HotDesk.Models;
using HotDesk.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDesk.Controllers
{
    [Route("api/location/{id}/desk")]
    [ApiController]
    [Authorize]
    public class DeskController : ControllerBase
    {
        private readonly IDeskServices _deskServices;

        public DeskController(IDeskServices deskServices)
        {
            _deskServices = deskServices;
        }

        [HttpGet("{deskId}")]
        public ActionResult<DeskDto> GetDesk([FromRoute] int id, [FromRoute] int deskId)
        {
            var deskDto = _deskServices.GetDeskById(id,deskId);
            return Ok(deskDto);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult AddDesk([FromRoute] int id, [FromBody] AddDeskDto dto)
        {
            int deskId = _deskServices.CreateDesk(id, dto);
            return Created($"api/location/{id}/desk/{deskId}", null);
        }

        [HttpDelete("{deskId}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteDesk([FromRoute] int id, [FromRoute] int deskId)
        {
            _deskServices.DeleteDesk(id, deskId);
            return NoContent();
        }
    }
}
