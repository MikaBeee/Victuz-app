using System.ComponentModel.DataAnnotations;

namespace Victuz.Models.Businesslayer
{
    public class Location
    {
        [Key]
        public int LocId {  get; set; }

        [Required]
        public string LocName { get; set; }
    }
}
