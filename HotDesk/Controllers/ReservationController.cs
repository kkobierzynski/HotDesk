using HotDesk.Entities;
using HotDesk.Models;
using HotDesk.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDesk.Controllers
{
    [Route("api/reservation")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationServices _reservationServices;
        public ReservationController(IReservationServices reservationServices)
        {
            _reservationServices = reservationServices;
        }

        [HttpGet("information/{locationId}")]
        public ActionResult<List<ReservationDto>> AllReservationsInLocation([FromRoute] int locationId)
        {
            var reservations = _reservationServices.GetReservedByLocation(locationId);
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public ActionResult<DeskBookDto> GetReservation([FromRoute] int id)
        {
            var reservation = _reservationServices.GetReservationById(id);
            return Ok(reservation);
        }

        [HttpPost]
        public ActionResult Reservation([FromBody] DeskBookDto dto)
        {
            int reservationId = _reservationServices.MakeReservation(dto);
            return Created($"api/reservation/{reservationId}", null);
        }

        [HttpPut("{id}")]
        public ActionResult ChangeReservation([FromRoute] int id, [FromBody] DeskBookDto dto)
        {
            _reservationServices.Update(id, dto);
            return Ok();
        }
    }
}
