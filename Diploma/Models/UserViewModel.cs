using Database.Entities;
using NuGet.Protocol.Plugins;

namespace Diploma.Models
{
    public class UserViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Patronymic { get; set; } = null!;
        public IEnumerable<Contract> Contracts { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public int RoleId { get; set; }
        public string Login { get; set; } = null!;

        public UserViewModel()
        {

        }

        public UserViewModel(User user)
        {
            ID = user.ID;
            Name = user.Name;
            Surname = user.Surname;
            Patronymic = user.Patronymic;
            Contracts = user.Contracts;
            RoleName = user.Role.Name;
            RoleId = user.RoleId;
            Login = user.Login;
        }

        public override string ToString() => $"ID {ID}\nLogin {Login}\nName {Name}\nSurname{Surname}\nPatronymic{Patronymic}";
    }
}
