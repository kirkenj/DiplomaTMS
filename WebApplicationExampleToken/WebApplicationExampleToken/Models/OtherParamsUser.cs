using System.ComponentModel.DataAnnotations;

namespace WebApplicationExampleToken.Models
{
    public class OtherParamsUser : ParamUser
    {
        public string UserName { get; set; }
        public bool SeniorManager { get; set; }
        [Required]
        public enumProfession Profession { get; set; }
    }
}
