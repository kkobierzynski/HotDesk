namespace HotDesk.Models
{
    public class DeskBookDto
    {
        public int LocationId { get; set; }
        public int DeskId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
