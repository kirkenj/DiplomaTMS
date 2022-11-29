using Database.Entities;
using Diploma.Models;
using Diploma.Models.Comands.Register;
using Diploma.Models.Constants;
using Diploma.Models.Interfaces;
using Diploma.Models.Queries.Login;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

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
            var result = await _accountService.TrySignInAsync(loginModel);
            if (!result.succed)
            {

                ViewBag.Message = "Invalid login or password";
                return View();
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, result.userLogin) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
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
            var result = await _accountService.TryCreateAsync(registerModel);
            if (result.succed)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Message = result.explanation;
                registerModel.PasswordStr= string.Empty;
                registerModel.ConfirmPasswordStr= string.Empty;
                return View(registerModel);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            StringValues strings = new();
            strings.Append("Bearer");
            Response.Headers.Authorization = strings;
            return Redirect("Home");
        }
    }
}
