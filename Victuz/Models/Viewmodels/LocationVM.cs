using System.ComponentModel.DataAnnotations;
using Victuz.Models.Businesslayer;

namespace Victuz.Models.Viewmodels
{
    public class LocationVM
    {
        public int LocId { get; set; }

        public string? LocName { get; set; }

        public ICollection<Gathering>? Gatherings { get; set; }
    }
}
