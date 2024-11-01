using System.ComponentModel.DataAnnotations;
using Victuz.Models.Businesslayer;

namespace Victuz.Models.Viewmodels
{
    public class CategoryVM
    {
        public int CatId { get; set; }

        public string? CatName { get; set; }

        public ICollection<Gathering>? Gatherings { get; set; }
    }
}
