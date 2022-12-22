using System.ComponentModel.DataAnnotations;

namespace HotDesk.Models
{
    public class AddDeskDto
    {
        [MaxLength(100)]
        public string? Description { get; set; }
        [Required]
        public int LocationId { get; set; }
    }
}
