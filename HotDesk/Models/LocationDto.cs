using HotDesk.Entities;

namespace HotDesk.Models
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public List<DeskDto> Desks { get; set; }
    }
}
