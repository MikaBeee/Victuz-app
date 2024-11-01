using System.ComponentModel.DataAnnotations;
using Victuz.Models.Businesslayer;

namespace Victuz.Models.Viewmodels
{
    public class ForumVM
    {
        public int ForumId { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }

        public ICollection<Post>? Posts { get; set; }
    }
}
