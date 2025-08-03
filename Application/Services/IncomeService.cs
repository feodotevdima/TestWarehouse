using Core.Exceptions;
using Core.Models;
using Persistence.Dto;

namespace Application.Interfaces
{
    public class IncomeService : IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IIncomeResourceRepository _incomeResourceRepository;
        private readonly IBalanceService _balanceService;

        public IncomeService(IIncomeRepository incomeRepository, IIncomeResourceRepository incomeResourceRepository, IBalanceService balanceService) 
        { 
            _incomeRepository = incomeRepository;
            _incomeResourceRepository = incomeResourceRepository;
            _balanceService = balanceService;
        }

        public async Task<Income> AddIncomeAsync(CreateIncomeDto dto)
        {
            var incomes = await _incomeRepository.GetFiltredIncomeDtosAsync(numbers: new List<string> { dto.Number });
            if (incomes != null && incomes.Any())
            {
                throw new ConflictException($"Невозможно создать документ поступления с номером {dto.Number}. Такой уже зарегистрирован в системе");
            }

            var income = new Income { Id = dto.Id, Number = dto.Number, Date = dto.Date };

            var balance = new List<Balance>();
            var resoures = new List<IncomeResource>();

            foreach(var item in dto.resources)
            {
                var b = new Balance { Id = Guid.NewGuid(), ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity };
                balance.Add(b);

                var r = new IncomeResource { Id = item.Id, IncomeId = income.Id, ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity };
                resoures.Add(r);
            }

            await _balanceService.AddBalanceAsync(balance);

            await _incomeRepository.AddIncomeAsync(income);
            await _incomeResourceRepository.AddIncomeResourceAsync(resoures);

            return income;
        }

        public async Task<Income> UpdateIncomeAsync(CreateIncomeDto dto)
        {
            var oldIncome = await _incomeRepository.GetIncomeByIdAsync(dto.Id);

            if (oldIncome == null)
            {
                throw new NotFoundException($"Документ поступления с ID {dto.Id} не найден");
            }

            if (dto.Number != oldIncome.Number)
            {
                var incomes = await _incomeRepository.GetFiltredIncomeDtosAsync(numbers: new List<string> { dto.Number });
                if (incomes != null && incomes.Any())
                {
                    throw new ConflictException($"Невозможно создать документ поступления с номером {dto.Number}. Такой уже зарегистрирован в системе");
                }
            }

            oldIncome.Number = dto.Number;
            oldIncome.Date = dto.Date;

            var existingResources = await _incomeResourceRepository.GetIncomeResourceByIncomeIdAsync(dto.Id);

            var oldBalance = new List<Balance>();
            var newBalance = new List<Balance>();

            foreach (var item in existingResources)
            {
                oldBalance.Add(new Balance { ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity });
            }

            foreach (var item in dto.resources)
            {
                newBalance.Add(new Balance { ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity });
            }

            await _balanceService.UpdateBalanceAsync(oldBalance, newBalance);

            await _incomeRepository.UpdateIncomeAsync(oldIncome);

            foreach (var item in existingResources)
            {
                await _incomeResourceRepository.RemoveIncomeResourceAsync(item.Id);
            }

            foreach (var item in dto.resources)
            {
                await _incomeResourceRepository.AddIncomeResourceAsync(new IncomeResource { Id = Guid.NewGuid(), IncomeId = oldIncome.Id, ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity });
            }


            return oldIncome;
        }

        public async Task<Income> DeleteIncomeAsync(Guid id)
        {
            var income = await _incomeRepository.GetIncomeByIdAsync(id);

            if (income == null)
                throw new NotFoundException($"Документ поступления с ID {id} не найден");

            var existingResources = await _incomeResourceRepository.GetIncomeResourceByIncomeIdAsync(id);

            var balance = new List<Balance>();
            foreach (var i in existingResources)
            {
                balance.Add(new Balance { ResourceId = i.ResourceId, UnitId = i.UnitId, Quantity = i.Quantity });
            }

            await _balanceService.RemoveBalanceAsync(balance);

            foreach (var item in existingResources)
            {
                await _incomeResourceRepository.RemoveIncomeResourceAsync(item.Id);
            }

            await _incomeRepository.RemoveIncomeAsync(income.Id);

            return income;
        }
    }
}
