using Database.Entities;
using Database.Interfaces;
using Diploma.Models.Comands.Register;
using Diploma.Models.Interfaces;
using Diploma.Models.Queries.Login;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Diploma.Models.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAppDBContext _context;
        private readonly HashAlgorithm _hashAlgorithm;
        private readonly Encoding _encoding;


        public AccountService(IAppDBContext context)
        {
            _context = context;
            _encoding = Encoding.UTF8;
            _hashAlgorithm = HashAlgorithm.Create("MD5") ?? throw new ArgumentException("Hash algorithm not found");
        }

        public async Task<(bool, string)> TryCreateAsync(RegisterUserModel createUserModel)
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

        public async Task<(bool, string)> TrySignInAsync(LoginUserModel loginUserModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == loginUserModel.Login);
            if (user == null) 
            {
                return (false, string.Empty);
            }

            var passwordHash = _encoding.GetString(GetHashCode(loginUserModel.PasswordStr));
            if (user.PasswordHash == passwordHash)
            {
                return (true, user.Login);
            }

            return (false, string.Empty);
        }

        //private string GetTokenAsync(User user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(nameof(user.Login), user.Login),
        //        new Claim(nameof(user.RoleId), user.RoleId.ToString())
        //    };

        //    var signinKey = new SymmetricSecurityKey(_encoding.GetBytes(_options.SecretKey));

        //    var jwt = new JwtSecurityToken
        //      (
        //          issuer: _options.Issuer,
        //          audience: _options.Audience,
        //          claims: claims,
        //          expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
        //          notBefore: DateTime.UtcNow,
        //          signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
        //      );

        //    return new JwtSecurityTokenHandler().WriteToken(jwt);
        //}

        private byte[] GetHashCode(string str)
        {
            var pwdBytes = _encoding.GetBytes(str ?? throw new ArgumentNullException(nameof(str)));
            var pwdHash = _hashAlgorithm.ComputeHash(pwdBytes);
            return pwdHash;
        }

        //bool Equals(byte[] a, byte[] b)
        //{
        //    if (a == null || b == null) return false;
        //    if (a.Length != b.Length) return false;
        //    for (int i = 0; i < a.Length; i++)
        //    {
        //        if (a[i] != b[i]) return false;
        //    }

        //    return true;
        //}
    }
}
