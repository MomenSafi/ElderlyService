using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ElderlyService.Models
{
    public class Reviews
    {
        public enum Status
        {
            Approved,
            pending,
            Rejected
        }
        [Key]
        [Required]
        public int ReviewID { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rate must be between 1 and 5.")]
        public int Rate { get; set; }
        [Required]
        public string Comment { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public Status status { get; set; } = Status.pending;
        public string userId { get; set; }
        public Users Users { get; set; }
        public string CaregiverId { get; set; }
        public Caregiver Caregiver { get; set; }
    }
}
