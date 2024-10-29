using System.ComponentModel.DataAnnotations;

namespace Victuz.Models.Businesslayer
{
    public class Category
    {
        [Key]
        public int CatId { get; set; }

        [Required]
        public string? CatName { get; set; }

        public ICollection<Gathering>? Gatherings { get; set; }
    }
}
