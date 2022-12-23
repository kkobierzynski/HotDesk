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

        [HttpPost("oneday")]
        public ActionResult OneDayReservation([FromBody] DeskOneDayBookDto dto)
        {
            int reservationId = _reservationServices.MakeOneDayReservation(dto);
            return Created($"api/reservation/{reservationId}", null);
        }
    }
}
