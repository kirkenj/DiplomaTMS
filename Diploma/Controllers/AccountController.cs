using Database.Interfaces;
using Diploma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAppDBContext _appDBContext = null!;

        public AccountController(IAppDBContext appDBContext1)
        {
            _appDBContext = appDBContext1;
        }

        public async Task<IActionResult> MyAccount()
        {
            var login = User.Identity?.Name ?? throw new ArgumentNullException();
            return View(new UserViewModel(await _appDBContext.Users.Include(y => y.Role).FirstAsync(u => u.Login == login)));
        }

        [Authorize("OnlyAdmin")]
        public IActionResult List()
        {
            var views = _appDBContext.Users.Include(y => y.Role).Select(u=>new UserViewModel(u)).ToList();
            return View(views);
        }

        [Authorize("OnlyAdmin")]
        [HttpPost]
        public async Task<IActionResult> SetRole(int userId, int RoleID)
        { 
            var roleTask = _appDBContext.Roles.FirstOrDefaultAsync(r => r.ID == RoleID);
            var userTask = _appDBContext.Users.FirstOrDefaultAsync(r => r.ID == userId);
            var role = roleTask.Result;
            var user = userTask.Result;
            if (role != null && user!=null) 
            {
                user.RoleId= role.ID;
                await _appDBContext.SaveChangesAsync();
            }
            
            return RedirectToAction("List");
        }
    }
}