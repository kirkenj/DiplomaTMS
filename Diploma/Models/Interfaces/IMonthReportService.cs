using Database.Entities;

namespace Diploma.Models.Interfaces
{
    public interface IMonthReportService
    {
        public Task<List<MonthReport>> GetMonthReportAsyncOnDate(DateTime date);
    }
}
