using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationExampleToken.Models;

namespace WebApplicationExampleToken.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JWTSettings _options;

        public AuthorizeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<JWTSettings> options)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options.Value; 
        }

        [HttpPost("Register")] 
        public async Task<IActionResult> Register(OtherParamsUser paramsUser)
        {
            var user = new IdentityUser{ UserName = paramsUser.UserName, Email = paramsUser.Email };

            var result = await _userManager.CreateAsync(user, paramsUser.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                List<Claim> claims = new()
                {
                    new Claim("Profession", paramsUser.Profession.ToString()),
                    new Claim("SeniorManager", paramsUser.SeniorManager.ToString()),
                    new Claim(ClaimTypes.Email, paramsUser.Email)
                };

                await _userManager.AddClaimsAsync(user, claims);
            }
            else
            {
                return BadRequest();
            }

            return Ok();
        }

        private string GetToken(IdentityUser user, IEnumerable<Claim> principal)
        {
            var claims = principal.ToList();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

            var jwt = new JwtSecurityToken
              (
                  issuer: _options.Issuer,
                  audience: _options.Audience,
                  claims: claims,
                  expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
                  notBefore: DateTime.UtcNow,
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
              );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(jwt);
            Request.Headers.Append("JWT", tokenStr);
            return tokenStr;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(ParamUser paramsUser)
        {
            var user = await _userManager.FindByEmailAsync(paramsUser.Email);

            var result = await _signInManager.PasswordSignInAsync(user, paramsUser.Password, false,false);

            if (result.Succeeded)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                var token = GetToken(user, claims);

                return Ok(token);
            }
        
            return BadRequest();
        }

    }
}
