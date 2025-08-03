using Core.Models;

namespace Application.Interfaces
{
    public interface IBalanceRepository
    {
        Task<IEnumerable<Balance>> GetFiltredBalanceAsync(List<Guid>? resourceIds = null, List<Guid>? unitIds = null);
        Task<Balance?> GetBalanceByIdAsync(Guid id);
        Task<Balance> AddBalanceAsync(Balance balance);
        Task<Balance> UpdateBalanceAsync(Balance balance);
        Task<Balance> RemoveBalanceAsync(Guid Id);
    }
}
