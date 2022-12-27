using HotDesk.Entities;

namespace HotDesk.Models
{
    public class ReservationDto
    {
        public int DeskId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public virtual UserDto User { get; set; }
    }
}
