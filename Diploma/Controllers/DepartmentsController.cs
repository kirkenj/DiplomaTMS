using Database.Interfaces;
using Diploma.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Controllers
{
    public class DepartmentsController : Controller
    {
        IDepartmentService _departmentService;
        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _departmentService.GetAll());
        }

        [Authorize("OnlyAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize("OnlyAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (_departmentService.FindByName(name) != null)
            {
                return BadRequest();
            }

            await _departmentService.Create(new Database.Entities.Department { Name = name });
            return RedirectToAction("Index");
        }

        [Authorize("OnlyAdmin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.FindByID(id);
            if (department is null)
            {
                return NotFound();
            }

            return View(department);
        }

        [Authorize("OnlyAdmin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string Name)
        {
            var res = await _departmentService.TryEdit(id, Name);
            if (!res)
            {
                return BadRequest();
            }

            return RedirectToAction("Index");
        }

        [Authorize("OnlySuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
