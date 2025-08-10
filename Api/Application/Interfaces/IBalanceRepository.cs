using Core.Models;
using Persistence.Dto;

namespace Application.Interfaces
{
    public interface IBalanceRepository
    {
        Task<IEnumerable<GetBalanceDto>> GetFiltredBalanceDtoAsync(List<Guid>? resourceIds = null, List<Guid>? unitIds = null);
        Task<IEnumerable<Balance>> GetFiltredBalanceAsync(List<Guid>? resourceIds = null, List<Guid>? unitIds = null);
        Task<Balance?> GetBalanceByIdAsync(Guid id);
        Task<IEnumerable<Resource>?> GetResourcesUsedInBalanceAsync();
        Task<IEnumerable<Unit>?> GetUnitsUsedInBalanceAsync();
        Task<Balance> AddBalanceAsync(Balance balance);
        Task<Balance> UpdateBalanceAsync(Balance balance);
        Task<Balance> RemoveBalanceAsync(Guid Id);
    }
}
