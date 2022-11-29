using System.ComponentModel.DataAnnotations;

namespace Diploma.Models.Comands.Register
{
    public class RegisterUserModel
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Surname { get; set; } = null!;
        [Required]
        public string Patronymic { get; set; } = null!;
        [Required]
        public string Login { get; set; } = null!;
        [Required]
        public string PasswordStr { get; set; } = null!;
        [Required]
        public string ConfirmPasswordStr { get; set; } = null!;
    }
}
