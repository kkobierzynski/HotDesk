using HotDesk.Entities;
using HotDesk.Models;
using HotDesk.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDesk.Controllers
{
    [Route("api/location")]
    [ApiController]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly ILocationServices _locationServices;
        public LocationController(ILocationServices locationServices)
        {
            _locationServices = locationServices;
        }

        [HttpGet]
        public ActionResult<IEnumerable<LocationDto>> GetDesksByLocalization([FromQuery] string searchWord)
        {
            var locations = _locationServices.GetLocations(searchWord);
            return Ok(locations);
        }

        [HttpGet("{id}")]
        public ActionResult<LocationDto> GetLocationById([FromRoute] int id)
        {
            var location = _locationServices.ReturnLocationById(id);
            return Ok(location);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult AddLocation(AddLocationDto dto)
        {
            var locationId = _locationServices.CreateLocation(dto);
            return Created($"api/location/{locationId}", null);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteLocation(int id)
        {
            _locationServices.DeleteLocation(id);
            return NoContent();
        }


    }
}
