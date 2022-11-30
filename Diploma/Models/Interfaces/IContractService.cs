using Database.Entities;

namespace Diploma.Models.Interfaces
{
    public interface IContractService
    {

        public Task Add(Contract contract);

        public Task<IEnumerable<Contract>> GetAll();

        public Task ConfirmContract(int id);

    }
}
