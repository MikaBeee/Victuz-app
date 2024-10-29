using System.ComponentModel.DataAnnotations;

namespace Victuz.Models.Businesslayer
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }

        [Required]
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public ICollection<Post>? Posts { get; set; }
        public ICollection<GatheringRegistration>? GatheringRegistrations { get; set; }
    }
}
