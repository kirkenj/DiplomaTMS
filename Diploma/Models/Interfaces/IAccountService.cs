using Database.Entities;
using Diploma.Models.Comands.Register;
using Diploma.Models.Queries.Login;
using System.Security.Claims;

namespace Diploma.Models.Interfaces
{
    public interface IAccountService
    {
        public Task<(bool succed, string explanation)> TryCreateAsync(RegisterUserModel createUserModel);
        public Task<(bool succed, string userLogin)> TrySignInAsync(LoginUserModel loginUserModel);
    }
}
