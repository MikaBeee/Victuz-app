using System.ComponentModel.DataAnnotations;

namespace Victuz.Models.Businesslayer
{
    public class ActivityRegistration
    {
        [Key]
        public int UserId { get; set; }
        public User User { get; set; }
        [Key]
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
