using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyService.Models
{
    public class Roles
    {
        [Key]
        [Required]
        public string RoleId { get; set; }
        [Required]
        public string TypeOfUser { get; set; }
        [ForeignKey("RoleId")]
        public ICollection<Users> Users { get; set; }
    }
}
