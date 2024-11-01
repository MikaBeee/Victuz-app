using System.ComponentModel.DataAnnotations;
using Victuz.Models.Businesslayer;

namespace Victuz.Models.Viewmodels
{
    public class GatheringRegistrationVM
    {
        public int UserId { get; set; }
        public User? User { get; set; }
        public int GatheringId { get; set; }
        public Gathering? Gathering { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
