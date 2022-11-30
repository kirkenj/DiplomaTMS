using Database.Entities;
using Database.Interfaces;
using Diploma.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Models.Services
{
    public class ContractService : IContractService
    {
        private readonly IAppDBContext _appDBContext;
        public ContractService(IAppDBContext appDBContext)
        {
            _appDBContext= appDBContext;
        }

        public async Task Add(Contract contract)
        {
            var res = contract.Validate(new System.ComponentModel.DataAnnotations.ValidationContext(contract));
            if (res.Any())
            {
                throw new Exception(string.Join("\n", res));
            }

            _appDBContext.Contracts.Add(contract);
            await _appDBContext.SaveChangesAsync();
        }

        private async Task AddMonthReports(Contract contract)
        {
            var dateStart = new DateTime(contract.PeriodStart.Year, contract.PeriodStart.Month, 1);
            var dateEnd = new DateTime(contract.PeriodEnd.Year, contract.PeriodEnd.Month, 1);
            var reports = new List<MonthReport>();
            while (dateStart <= dateEnd)
            {
                reports.Add(new MonthReport { Contract = contract, ContractID = contract.ID, Month = dateStart.Month, Year = dateStart.Year });
                dateStart = dateStart.AddMonths(1);
            }

            _appDBContext.MonthReports.AddRange(reports);
            await _appDBContext.SaveChangesAsync();
        }

        public async Task ConfirmContract(Contract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            contract.IsConfirmed = true;
            await _appDBContext.SaveChangesAsync();
            await AddMonthReports(contract);
        }

        public async Task<IEnumerable<Contract>> GetAll()
        {
            return await _appDBContext.Contracts.Include(c => c.User).Include(c=>c.Department).ToListAsync();
        }

        public async Task<Contract?> GetById(int id)=> await _appDBContext.Contracts.Include(c=>c.User).Include(c=>c.Department).Include(c=>c.MonthReports).FirstOrDefaultAsync(c => c.ID == id);
    
        private bool IsValidIfReplace(Contract contract, MonthReport monthReport)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            if (monthReport == null)
            {
                throw new ArgumentNullException(nameof(monthReport));
            }

            var reports = contract.MonthReports.ToList();
            var report = reports.FirstOrDefault(r => r.ContractID == monthReport.ContractID && r.Month == monthReport.Month && r.Year == monthReport.Year);
            if (report == null)
            {
                throw new ArgumentException(nameof(report));
            }

            reports.Remove(report);
            reports.Add(monthReport);
            if (reports.Sum(r=>r.TimeSum) > contract.TimeSum)
            {
                throw new Exception("reports.Sum(r=>r.TimeSum) > contract.TimeSum");
            }

            if (reports.Sum(r=>r.LectionsTime) > contract.LectionsMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.LectionsTime) > contract.LectionsMaxTime");

            }

            if (reports.Sum(r=>r.PracticalClassesTime) > contract.PracticalClassesMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.PracticalClassesTime) > contract.PracticalClassesMaxTime");

            }

            if (reports.Sum(r=>r.LaboratoryClassesTime) > contract.LaboratoryClassesMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.LaboratoryClassesTime) > contract.LaboratoryClassesMaxTime");
            }

            if (reports.Sum(r=>r.ConsultationsTime) > contract.ConsultationsMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.ConsultationsTime) > contract.ConsultationsMaxTime");
            }

            if (reports.Sum(r=>r.OtherTeachingClassesTime) > contract.OtherTeachingClassesMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.OtherTeachingClassesTime) > contract.OtherTeachingClassesMaxTime");
            }

            if (reports.Sum(r=>r.CreditsTime) > contract.CreditsMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.CreditsTime) > contract.CreditsMaxTime");
            }

            if (reports.Sum(r=>r.ExamsTime) > contract.ExamsMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.ExamsTime) > contract.ExamsMaxTime");
            }

            if (reports.Sum(r=>r.CourseProjectsTime) > contract.CourseProjectsMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.CourseProjectsTime) > contract.CourseProjectsMaxTime");
            }

            if (reports.Sum(r=>r.InterviewsTime) > contract.InterviewsMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.InterviewsTime) > contract.InterviewsMaxTime");
            }

            if (reports.Sum(r=>r.TestsAndReferatsTime) > contract.TestsAndReferatsMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.TestsAndReferatsTime) > contract.TestsAndReferatsMaxTime");
            }

            if (reports.Sum(r=>r.InternshipsTime) > contract.InternshipsMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.InternshipsTime) > contract.InternshipsMaxTime");
            }

            if (reports.Sum(r=>r.DiplomasTime) > contract.DiplomasMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.DiplomasTime) > contract.DiplomasMaxTime");
            }

            if (reports.Sum(r=>r.DiplomasReviewsTime) > contract.DiplomasReviewsMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.DiplomasReviewsTime) > contract.DiplomasReviewsMaxTime");
            }

            if (reports.Sum(r=>r.SECTime) > contract.SECMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.SECTime) > contract.SECMaxTime");
            }

            if (reports.Sum(r=>r.GraduatesManagementTime) > contract.GraduatesManagementMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.GraduatesManagementTime) > contract.GraduatesManagementMaxTime");
            }

            if (reports.Sum(r=>r.GraduatesAcademicWorkTime) > contract.GraduatesAcademicWorkMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.GraduatesAcademicWorkTime) > contract.GraduatesAcademicWorkMaxTime");
            }

            if (reports.Sum(r=>r.PlasticPosesDemonstrationTime) > contract.PlasticPosesDemonstrationMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.PlasticPosesDemonstrationTime) > contract.PlasticPosesDemonstrationMaxTime");
            }

            if (reports.Sum(r=>r.TestingEscortTime) > contract.TestingEscortMaxTime)
            {
                throw new Exception("reports.Sum(r=>r.TestingEscortTime) > contract.TestingEscortMaxTime");
            }

            return true;
        }


        public async Task UpdateMonthReport(MonthReport monthReport)
        {
            var res = monthReport.Validate(new System.ComponentModel.DataAnnotations.ValidationContext(monthReport));
            if (res.Any())
            {
                throw new Exception(string.Join("\n", res));
            }

            var contract = await GetById(monthReport.ContractID);
            if (contract == null)
            {
                throw new Exception($"Not found {nameof(contract)}");
            }

            if (IsValidIfReplace(contract, monthReport))
            {
                var report = await _appDBContext.MonthReports.FirstOrDefaultAsync(r => r.ContractID == monthReport.ContractID && r.Month == monthReport.Month && r.Year == monthReport.Year);
                _appDBContext.MonthReports.Remove(report ?? throw new ArgumentNullException(nameof(report)));
                _appDBContext.MonthReports.Add(monthReport);
                await _appDBContext.SaveChangesAsync();
            }
        }
    }
}
