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

        [HttpPost]
        public ActionResult Reservation([FromBody] DeskBookDto dto)
        {
            int reservationId = _reservationServices.MakeReservation(dto);
            return Created($"api/reservation/{reservationId}", null);
        }
    }
}
