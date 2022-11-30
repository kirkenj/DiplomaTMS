using Database.Interfaces;
using Diploma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Diploma.Models.Interfaces;

namespace Diploma.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService = null!;

        public AccountController(IAccountService appDBContext1)
        {
            _accountService = appDBContext1;
        }

        public async Task<IActionResult> MyAccount()
        {
            var login = User.Identity?.Name ?? throw new ArgumentNullException();
            return View(new UserViewModel(await _accountService.GetUserByLogin(login)));
        }

        [Authorize("OnlyAdmin")]
        public IActionResult List()
        {
            var views = _accountService.GetUserViewModelsList();
            return View(views);
        }

        [Authorize("OnlyAdmin")]
        public async Task<IActionResult> SetRole(int userId, int RoleID)
        { 
            await _accountService.SetRole(userId, RoleID);
            return RedirectToAction("List");
        }
    }
}