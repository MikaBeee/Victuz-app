using System.ComponentModel.DataAnnotations;
using Victuz.Models.Businesslayer;

namespace Victuz.Models.Viewmodels
{
    public class PostVM
    {
        public int PostId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ForumId { get; set; }
        public Forum Forum { get; set; }

        public string Content { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.Now;
    }
}
