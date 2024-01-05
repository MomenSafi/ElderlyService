using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyService.Models
{
    public class Users
    {
        [Key]
        [Required]
        public string userId { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        [DisplayName("Date of birth")]
        public DateTime? DateOfBirth { get; set; }
        public int Gender { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; }

        [NotMapped]
        public bool IsCaregiver { get; set; } = false;
        public string RoleId { get; set; }
        public Roles Roles { get; set; }
        [ForeignKey("userId")]
        public ICollection<Reviews> Reviews { get; set; }
        [ForeignKey("userId")]
        public ICollection<Caregiver> caregivers { get; set; }
        [ForeignKey("userId")]
        public ICollection<Notification> Notifications { get; set; }
        [ForeignKey("userId")]
        public ICollection<Appointment> Appointments { get; set; }
        [ForeignKey("userId")]
        public ICollection<ReviewsForWebsites> ReviewsForWebsites { get; set; }

    }
}
