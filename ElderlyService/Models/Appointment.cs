using System.ComponentModel.DataAnnotations;

namespace ElderlyService.Models
{
    public class Appointment
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
        public int AppointmentId { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set;}
        public int status { get; set; }
        [Required]
        public DateTime BookingDate { get; set; }
        public Days DayOfWeek { get; set; }
        public string Notes { get; set; }
        [Url]
        [Required]
        public string Location { get; set; }
        public string userId { get; set; }
        public Users Users { get; set; }
        public string CaregiverId { get; set; }
        public Caregiver Caregiver { get; set; }

    }
}
