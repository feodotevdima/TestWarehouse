using Core.Models;

namespace Application.Interfaces
{
    public interface IIncomeResourceRepository
    {
        Task<IEnumerable<IncomeResource>?> GetIncomeResourcesByUnitIdAsync(Guid unitId);
        Task<IEnumerable<IncomeResource>?> GetIncomeResourcesByResourceIdAsync(Guid Id);
        Task<IEnumerable<IncomeResource>?> GetIncomeResourceByIncomeIdAsync(Guid incomeId);
        Task<IEnumerable<Resource>?> GetResourcesUsedInIncomeAsync();
        Task<IEnumerable<Unit>?> GetUnitsUsedInIncomeAsync();
        Task<IncomeResource> AddIncomeResourceAsync(IncomeResource shipmentResource);
        Task<IEnumerable<IncomeResource>> AddIncomeResourceAsync(IEnumerable<IncomeResource> incomeResources);
        Task<IncomeResource> UpdateIncomeResourceAsync(IncomeResource shipmentResource);
        Task<IncomeResource> RemoveIncomeResourceAsync(Guid Id);
    }
}
