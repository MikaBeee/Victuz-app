using System.ComponentModel.DataAnnotations;

namespace Victuz.Models.Businesslayer
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required(ErrorMessage = "User is verplicht.")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required(ErrorMessage = "Forum is verplicht.")]
        public int ForumId { get; set; }
        public Forum? Forum { get; set; }

        [Required(ErrorMessage = "Content is verplicht.")]
        public string Content { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.Now;
    }
}
