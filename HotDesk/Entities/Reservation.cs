namespace HotDesk.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int DeskId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        public int UserId { get; set; }
    }
}
