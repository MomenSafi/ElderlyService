using System.ComponentModel.DataAnnotations;

namespace ElderlyService.Models
{
    public class CardData
    {
        [Key]
        public string CardId { get; set; }
        public string password { get; set; }
       
    }

}
