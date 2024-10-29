using System.ComponentModel.DataAnnotations;

namespace Victuz.Models.Businesslayer
{
    public class User
    {
        [Key]
        public int userId { get; set; }

        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }

        [Required]
        public int roleId { get; set; }
        public Role role { get; set; }
    }
}
