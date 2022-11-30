using Database;
using Database.Entities;
using Database.Interfaces;
using Diploma.Models.Comands.Register;
using Diploma.Models.Interfaces;
using Diploma.Models.Queries.Login;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Security.Cryptography;
using System.Text;

namespace Diploma.Models.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAppDBContext _context;
        private readonly HashAlgorithm _hashAlgorithm = HashAlgorithm.Create("MD5") ?? throw new ArgumentException("Hash algorithm not found");
        private readonly Encoding _encoding = Encoding.UTF8;

        public AccountService(IAppDBContext context)
        {
            _context = context;
        }

        public async Task<(bool, string)> CreateAsync(RegisterUserModel createUserModel)
        {
            if (createUserModel is null)
            {
                return (false, $"{nameof(createUserModel)} is null");
            }

            if (await _context.Users.AnyAsync(u=>u.Login == createUserModel.Login))
            {
                return (false, "This login is taken");
            }

            var strB = GetHashCode(createUserModel.PasswordStr);
            var userEntity = new User { Login= createUserModel.Login, Name = createUserModel.Name, Surname = createUserModel.Surname, Patronymic = createUserModel.Patronymic, PasswordHash = _encoding.GetString(strB)};
            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();
            return (true, $"User {userEntity.Login} created");
        }

        public async Task<Role> GetRoleByID(int id)
        {
            return await _context.Roles.FirstAsync(u => u.ID== id);
        }

        public async Task<User> GetUserByID(int id)
        {
            return await _context.Users.Include(y => y.Role).FirstAsync(u => u.ID == id);
        }

        public async Task<User> GetUserByLogin(string Login)
        {
            return await _context.Users.Include(y => y.Role).FirstAsync(u => u.Login == Login);
        }

        public List<UserViewModel> GetUserViewModelsList()
        {
            return _context.Users.Include(y => y.Role).Select(u => new UserViewModel(u)).ToList();
        }

        public async Task SetRole(int userId, int roleId)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.ID == roleId);
            var user = await _context.Users.FirstOrDefaultAsync(r => r.ID == userId);
            if (role != null && user != null)
            {
                user.RoleId = role.ID;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserViewModel?> SignInAsync(LoginUserModel loginUserModel)
        {
            var user = await _context.Users.Include(u => u.Role).Include(u=>u.Contracts).FirstOrDefaultAsync(u => u.Login == loginUserModel.Login);
            if (user == null) 
            {
                return null;
            }

            var passwordHash = _encoding.GetString(GetHashCode(loginUserModel.PasswordStr));
            if (user.PasswordHash == passwordHash)
            {
                return new UserViewModel(user);
            }

            return null;
        }

        private byte[] GetHashCode(string str)
        {
            var pwdBytes = _encoding.GetBytes(str ?? throw new ArgumentNullException(nameof(str)));
            var pwdHash = _hashAlgorithm.ComputeHash(pwdBytes);
            return pwdHash;
        }
    }
}
