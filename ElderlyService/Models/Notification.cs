using System.ComponentModel.DataAnnotations;

namespace ElderlyService.Models
{
    public class Notification
    {
        [Key]
        [Required]
        public int NotificationId { get; set; }
        public string content { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public bool IsRead { get; set; }
        public string userId { get; set; }
        public Users users { get; set; }
    }
}
