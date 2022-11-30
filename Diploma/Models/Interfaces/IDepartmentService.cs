using Database.Entities;

namespace Diploma.Models.Interfaces
{
    public interface IDepartmentService
    {
        public Task<Department?> FindByName(string name);
        public Task<Department?> FindByID(int id);
        public Task<bool> TryEdit(int id, string newName);
        public Task Create(Department department);
        public Task Delete(int id);
        public Task<List<Department>> GetAll();
    }
}
