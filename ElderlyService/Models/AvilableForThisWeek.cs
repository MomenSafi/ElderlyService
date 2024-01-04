using System.ComponentModel.DataAnnotations;

namespace ElderlyService.Models
{
    public class AvilableForThisWeek
    {
        public enum Days
        {
            Saturday,
            Sunday,
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday
        }
        [Key]
        [Required]
        public int AvilableForThisWeekID { get; set; }
        [Required]
        public Days DayOfWeek { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        public string CaregiverId { get; set; }
        public Caregiver Caregiver { get; set; }
    }
}
