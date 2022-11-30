using Database.Entities;
using Diploma.Models.Comands.Register;
using Diploma.Models.Queries.Login;
using System.Security.Claims;

namespace Diploma.Models.Interfaces
{
    public interface IAccountService
    {
        public Task<(bool succed, string explanation)> CreateAsync(RegisterUserModel createUserModel);
        public Task<UserViewModel?> SignInAsync(LoginUserModel loginUserModel);
        public Task<User> GetUserByLogin(string Login);
        public Task<User> GetUserByID(int id);
        public Task<Role> GetRoleByID(int id);
        public List<UserViewModel> GetUserViewModelsList();
        public Task SetRole(int userId, int roleId);
    }
}
