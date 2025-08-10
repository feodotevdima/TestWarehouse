using Core.Models;

namespace Application.Interfaces
{
    public interface IUnitRepository
    {
        Task<IEnumerable<Unit>> GetAllUnitsAsync();
        Task<IEnumerable<Unit>> GetActiveUnitsAsync();
        Task<IEnumerable<Unit>> GetArchiveUnitsAsync();
        Task<Unit?> GetUnitByIdAsync(Guid id);
        Task<Unit?> GetUnitByNameAsync(string name);
        Task<Unit> AddUnitAsync(Unit unit);
        Task<Unit> UpdateUnitAsync(Unit unit);
        Task<Unit> RemoveUnitAsync(Guid Id);
    }
}
