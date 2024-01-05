using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyService.Models
{
    public class Caregiver 
    {
        [Key]
        [Required]
        public string CaregiverId { get; set; }

        [DisplayName("Price of Service per hour")]
        public int? PriceOfService { get; set; }
        public bool? Valid { get; set; } = false;
        public DateTime? EndSubscribe { get; set; }
        public string? AboutYou { get; set; }
        public string? userId { get; set; }
        public Users Users { get; set; }
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }
        [ForeignKey("CaregiverId")]
        public ICollection<Reviews> Reviews { get; set; }
        [ForeignKey("CaregiverId")]
        public ICollection<Appointment> Appointments { get; set; }
        [ForeignKey("CaregiverId")]
        public ICollection<Experience> Experiences { get; set; }
        [ForeignKey("CaregiverId")]
        public ICollection<Availability> Availabilities { get; set; }
        [ForeignKey("CaregiverId")]
        public ICollection<AvilableForThisWeek> AvilableForThisWeek { get; set; }
        [ForeignKey("CaregiverId")]
        public ICollection<Payment> payments { get; set; }
        [ForeignKey("CaregiverId")]
        public ICollection<CardData> CardDatas { get; set; }



    }
}
