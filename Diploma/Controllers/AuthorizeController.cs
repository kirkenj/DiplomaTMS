using Diploma.Models.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Diploma.Models;

namespace Diploma.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly IAccountService _accountService; 

        public AuthorizeController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginUserModel loginModel)
        {
            var result = await _accountService.SignInAsync(loginModel);
            if (result == null)
            {

                ViewBag.Message = "Invalid login or password";
                return View();
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, result.Login), new Claim(ClaimTypes.Role, result.RoleName), new Claim(type:"RoleID", value: result.RoleId.ToString()) };
            ClaimsIdentity claimsIdentity = new(claims, "Cookies");
            await Request.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToAction("Index", "Home");
        }

        // GET: AuthorizeControlle/Create
        [HttpGet("Register")]
        public ActionResult Register()
        {
            return View();
        }

        // POST: AuthorizeControlle/Create
        [HttpPost]
        public async Task<ActionResult> Register(RegisterUserModel registerModel)
        {
            
            var q = registerModel.Validate(new System.ComponentModel.DataAnnotations.ValidationContext(User));
            if (q.Any()) 
            {
                throw new ArgumentException(string.Join("\n", q));
            }


            var result = await _accountService.CreateAsync(registerModel);
            if (result.succed)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Message = result.explanation;
                return View(registerModel);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            Request.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
