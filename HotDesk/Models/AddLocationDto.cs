using System.ComponentModel.DataAnnotations;

namespace HotDesk.Models
{
    public class AddLocationDto
    {
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        public string? PostalCode { get; set; }
    }
}
