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
            _appDBContext.Contracts.Add(contract);
            await _appDBContext.SaveChangesAsync();
        }

        public async Task ConfirmContract(int id)
        {
            var contract = _appDBContext.Contracts.FirstOrDefault(c => c.ID == id);
            if (contract == null)
            {
                throw new ArgumentNullException($"Contract with id {id} does not exist");
            }

            contract.IsConfirmed = true;
            await _appDBContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Contract>> GetAll()
        {
            return await _appDBContext.Contracts.Include(c => c.User).Include(c=>c.Department).ToListAsync();
        }
    }
}
