using System.ComponentModel.DataAnnotations;

namespace Victuz.Models.Businesslayer
{
    public class GatheringRegistration
    {
        [Key]
        public int UserId { get; set; }
        public User? User { get; set; }
        [Key]
        public int GatheringId { get; set; }
        public Gathering? Gathering { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
