using Database.Entities;
using Diploma.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Diploma.Controllers
{
    [Authorize]
    public class ContractsController : Controller
    {
        readonly IDepartmentService _departmentService;
        readonly IAccountService _accountService;
        readonly IContractService _contractService;

        public ContractsController(IDepartmentService departmentService, IAccountService accountService, IContractService contractService)
        {
            _departmentService = departmentService;
            _accountService = accountService;
            _contractService = contractService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ShowUsersContracts() 
        {
            var currentUser = await _accountService.GetUserByLogin(User.Identity?.Name ?? throw new UnauthorizedAccessException());
            return View(currentUser.Contracts.ToList());
        }

        public async Task<IActionResult> Create()
        {
            var dList = (await _departmentService.GetAll()).Select(e => new SelectListItem { Value = e.ID.ToString(), Text = e.Name }).ToList();
            ViewData["List"] = dList;
            var currentUser = await _accountService.GetUserByLogin(User.Identity?.Name ?? throw new UnauthorizedAccessException());
            var contract = new Contract { UserID = currentUser.ID };
            return View(contract);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Contract contract)
        {
            if (contract == null || contract.TimeSum <= 0)
            {
                return BadRequest();
            }

            await _contractService.Add(contract);
            return RedirectToAction("Index");
        }

        [Authorize("OnlyAdmin")]
        public async Task<IActionResult> AdminContractsPanel()
        {
            return View((await _contractService.GetAll()).ToList());
        }

        [Authorize("OnlyAdmin")]
        public async Task<IActionResult> Confirm(int contractID)
        {
            await _contractService.ConfirmContract(contractID);
            return RedirectToAction("AdminContractsPanel");
        }
    }
}
