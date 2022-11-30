using Database.Entities;
using Database.Interfaces;
using Diploma.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Models.Services
{
    public class MonthReportService : IMonthReportService
    {
        IAppDBContext _appDBContext;
        public MonthReportService(IAppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public async Task<List<MonthReport>> GetMonthReportAsyncOnDate(DateTime date)
        {
            return await _appDBContext.MonthReports.Include(m=>m.Contract).ThenInclude(c=>c.User).Where(m => m.Year == date.Year && m.Month == date.Month).ToListAsync();
        }
    }
}
