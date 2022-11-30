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
        readonly IMonthReportService _monthReportService;

        public ContractsController(IDepartmentService departmentService, IAccountService accountService, IContractService contractService, IMonthReportService monthReportService)
        {
            _departmentService = departmentService;
            _accountService = accountService;
            _contractService = contractService;
            _monthReportService = monthReportService;
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
            var contract = await _contractService.GetById(contractID);
            if (contract == null) 
            {
                return BadRequest(); 
            }
            await _contractService.ConfirmContract(contract);
            return RedirectToAction("AdminContractsPanel");
        }


        [Authorize]
        public async Task<IActionResult> Details(int contractID)
        {
            var contract = await _contractService.GetById(contractID);
            if (contract == null)
            {
                return BadRequest();
            }

            return View(contract);
        }

        [Authorize]
        public async Task<IActionResult> EditMonthReport(int contractID, int month, int year)
        {
            var contract = await _contractService.GetById(contractID);
            if (contract is null)
            {
                return NotFound(contractID);
            }
            var report = contract.MonthReports.FirstOrDefault(r => r.Month == month && r.Year == year);
            if (report is null)
            {
                return NotFound(report);
            }

            return View(report);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditMonthReport(MonthReport monthReport)
        {
            await _contractService.UpdateMonthReport(monthReport);
            return RedirectToAction("Details", new { contractID = monthReport.ContractID });
        }

        [Authorize("OnlyAdmin")]
        public IActionResult GetReportOnMonth()
        {
            return View();
        }

        [Authorize("OnlyAdmin")]
        [HttpPost]
        public async Task<IActionResult> GetReportOnMonthView(DateTime date)
        {
            var res = await _monthReportService.GetMonthReportAsyncOnDate(date);
            return View("GetReportOnMonthView", res);
        }
    }
}
