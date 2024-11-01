using System.ComponentModel.DataAnnotations;
using Victuz.Models.Businesslayer;

namespace Victuz.Models.Viewmodels
{
    public class GatheringVM
    {
        //eruit halen wat niet nodig is wanneer wireframes af zijn (ook doen in controller GatheringController -> AllGatherings)
        public int GatheringId { get; set; }

        public string? GatheringTitle { get; set; }
        public string? GatheringDescription { get; set; }

        public int MaxParticipants { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int LocationId { get; set; }
        public Location? Location { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string? Photopath { get; set; }
        public ICollection<GatheringRegistration>? GatheringRegistrations { get; set; }
    }
}
