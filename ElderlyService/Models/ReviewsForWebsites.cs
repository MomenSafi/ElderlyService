using System.ComponentModel.DataAnnotations;

namespace ElderlyService.Models
{
    public class ReviewsForWebsites
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
        public string Comment { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public Status status { get; set; } = Status.pending;
        public string userId { get; set; }
        public Users Users { get; set; }
    }
}
