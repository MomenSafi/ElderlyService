using System.ComponentModel.DataAnnotations;

namespace ElderlyService.Models
{
    public class Payment
    {
        public enum Status
        {
            Approved,
            pending,
            Rejected
        }
        [Key]
        [Required]
        public int PaymentId { get; set; }
        [Required]
        public int Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public Status status { get; set; } = Status.pending;
        public string CaregiverId { get; set; }
        public Caregiver Caregiver { get; set; }
    }
}
