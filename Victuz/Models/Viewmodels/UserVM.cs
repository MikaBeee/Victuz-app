using System.ComponentModel.DataAnnotations;
using Victuz.Models.Businesslayer;

namespace Victuz.Models.Viewmodels
{
    public class UserVM
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public ICollection<Post>? Posts { get; set; }
        public ICollection<GatheringRegistration>? GatheringRegistrations { get; set; }
    }
}
