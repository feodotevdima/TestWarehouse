using Core.Models;
using Persistence.Dto;

namespace Application.Interfaces
{
    public interface IUnitService
    {
        Task<Unit> ChangeUnitStatusAsync(Guid id);
        Task<Unit> AddUnitAsync(CreateUnitDto dto);
        Task<Unit> UpdateUnitAsync(CreateUnitDto dto);
        Task<Unit> DeleteUnitAsync(Guid id);
    }
}
