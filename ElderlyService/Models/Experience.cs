using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElderlyService.Models
{
    public class Experience
    {
        [Key]
        [Required]
        public int ExperienceId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [DisplayName("place of work")]
        public string placeOfExperience { get; set; }
        public string CaregiverId { get; set; }
        public Caregiver Caregiver { get; set; }
    }
}
