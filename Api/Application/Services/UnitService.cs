using Application.Interfaces;
using Core.Exceptions;
using Core.Models;
using Persistence.Dto;

namespace Application.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IShipmentResourceRepository _shipmentResourceRepository;
        private readonly IIncomeResourceRepository _incomeResourceRepository;
        private readonly IBalanceRepository _balanceRepository;

        public UnitService(IUnitRepository unitRepository, IShipmentResourceRepository shipmentResourceRepository, IIncomeResourceRepository incomeResourceRepository, IBalanceRepository balanceRepository) 
        {
            _unitRepository = unitRepository;
            _shipmentResourceRepository = shipmentResourceRepository;
            _incomeResourceRepository = incomeResourceRepository;
            _balanceRepository = balanceRepository;
        }

        public async Task<Unit> ChangeUnitStatusAsync(Guid id)
        {
            var unit = await _unitRepository.GetUnitByIdAsync(id);
            unit.IsActive = !unit.IsActive;
            await _unitRepository.UpdateUnitAsync(unit);
            return unit;
        }

        public async Task<Unit> AddUnitAsync(CreateUnitDto dto)
        {
            var unit = await _unitRepository.GetUnitByNameAsync(dto.Name);
            if (unit != null)
            {
                throw new ConflictException($"Невозможно создать единицу измерения {dto.Name}. Такая уже зарегистрирована в системе");
            }

            unit = new Unit { Id =  dto.Id, Name = dto.Name };
            await _unitRepository.AddUnitAsync(unit);
            return unit;
        }

        public async Task<Unit> UpdateUnitAsync(CreateUnitDto dto)
        {
            var unit = await _unitRepository.GetUnitByIdAsync(dto.Id);

            if (unit.Name != dto.Name)
            {
                if (await _unitRepository.GetUnitByNameAsync(dto.Name) != null)
                {
                    throw new ConflictException($"Невозможно создать единицу измерения {dto.Name}. Такая уже зарегистрирована в системе");
                }
            }

            var newUnit = new Unit { Id = dto.Id, Name = dto.Name, IsActive = dto.IsActive };
            await _unitRepository.UpdateUnitAsync(newUnit);
            return newUnit;
        }

        public async Task<Unit> DeleteUnitAsync(Guid id)
        {
            if(await CheckUnitToUse(id))
                throw new ConflictException($"Невозможно удалить единицу измерения, так как она используется в системе");

            var unit = await _unitRepository.RemoveUnitAsync(id);
            return unit;
        }

        public async Task<bool> CheckUnitToUse( Guid id)
        {
            var shipmentsResource = await _shipmentResourceRepository.GetShipmentResourcesByUnitIdAsync(id);
            var incomeResource = await _incomeResourceRepository.GetIncomeResourcesByUnitIdAsync(id);
            var balance = await _balanceRepository.GetFiltredBalanceAsync(unitIds: new List<Guid> { id });

            return (shipmentsResource != null || incomeResource != null || (balance != null && balance.Any()));
        }
    }
}
