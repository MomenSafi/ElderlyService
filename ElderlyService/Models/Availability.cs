using System.ComponentModel.DataAnnotations;

namespace ElderlyService.Models
{
    public class Availability
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
        public int AvailabilityID { get; set; }
        [Required]
        public Days DayOfWeek { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        public int Status { get; set; } = 1;
        public DateTime Date { get; set; }
        public string CaregiverId { get; set; }
        public Caregiver Caregiver { get; set; }

    }
}
