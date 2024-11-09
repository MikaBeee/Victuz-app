using System.ComponentModel.DataAnnotations;

namespace Victuz.Models.Businesslayer
{
    public class Vote
    {
        [Key]
        [Required]
        public int GatheringId { get; set; }
        public Gathering? Gathering { get; set; }

        [Key]
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
