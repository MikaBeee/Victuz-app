using System.ComponentModel.DataAnnotations;

namespace Victuz.Models.Businesslayer
{
    public class Gathering
    {
        [Key]
        public int GatheringId {  get; set; }

        [Required]
        public string? GatheringTitle { get; set; }
        public string? GatheringDescription { get; set; }
        [Required]
        public int MaxParticipants { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public int LocationId { get; set; }
        public Location? Location { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string? Photopath { get; set; }

        public ICollection<GatheringRegistration>? GatheringRegistrations { get; set; }
    }
}
