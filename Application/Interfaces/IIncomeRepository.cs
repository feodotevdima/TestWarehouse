using Core.Models;
using Persistence.Dto;

namespace Application.Interfaces
{
    public interface IIncomeRepository
    {
        Task<IEnumerable<GetIncomeDto>> GetFiltredIncomeDtosAsync(DateTime? start = null, DateTime? end = null, List<string>? numbers = null, List<Guid>? resourceIds = null, List<Guid>? unitIds = null);
        Task<GetIncomeDto?> GetIncomeDtoByIdAsync(Guid id);
        Task<Income?> GetIncomeByIdAsync(Guid id);
        Task<Income> AddIncomeAsync(Income income);
        Task<Income> UpdateIncomeAsync(Income income);
        Task<Income> RemoveIncomeAsync(Guid Id);
    }
}
