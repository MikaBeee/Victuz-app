using System.ComponentModel.DataAnnotations;

namespace Victuz.Models.Businesslayer
{
    public class Activity
    {
        [Key]
        public int ActivityId {  get; set; }

        [Required]
        public string ActivityTitle { get; set; }
        public string ActivityDescription { get; set; }
        [Required]
        public int MaxParticipants { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public int LocationId { get; set; }
        public Location Location { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
