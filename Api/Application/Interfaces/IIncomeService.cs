using Core.Models;
using Persistence.Dto;

namespace Application.Interfaces
{
    public interface IIncomeService
    {
        Task<Income> AddIncomeAsync(CreateIncomeDto dto);
        Task<Income> UpdateIncomeAsync(CreateIncomeDto dto);
        Task<Income> DeleteIncomeAsync(Guid id);
    }
}
