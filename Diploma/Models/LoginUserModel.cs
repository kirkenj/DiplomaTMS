using System.ComponentModel.DataAnnotations;

namespace Diploma.Models
{
    public class LoginUserModel
    {
        [Required]
        public string Login { get; set; } = null!;
        [Required]
        public string PasswordStr { get; set; } = null!;
    }
}
