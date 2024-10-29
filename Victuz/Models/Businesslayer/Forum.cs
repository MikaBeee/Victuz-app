using System.ComponentModel.DataAnnotations;

namespace Victuz.Models.Businesslayer
{
    public class Forum
    {
        [Key]
        public int ForumId { get; set; }

        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }

        public ICollection<Post>? Posts { get; set; }
    }
}
