using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyService.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; } 
        public string? ContentType { get; set; } 
        public string? Description { get; set; }
        [ForeignKey("ServiceId")]
        public ICollection<Caregiver> Caregivers { get; set; }

    }
}
