using System.ComponentModel.DataAnnotations;

namespace Victuz.Models.Viewmodels
{
    public class RegisterVM
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Wachtwoord moet minimaal 8 tekens lang zijn")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Wachtwoord moet minimaal 8 tekens lang zijn")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
