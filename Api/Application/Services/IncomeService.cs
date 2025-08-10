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
        private readonly IUnitRepository _unitRepository;
        private readonly IResourceRepository _resourceRepository;

        public IncomeService(IIncomeRepository incomeRepository, IIncomeResourceRepository incomeResourceRepository, IBalanceService balanceService, IResourceRepository resourceRepository, IUnitRepository unitRepository)
        {
            _incomeRepository = incomeRepository;
            _incomeResourceRepository = incomeResourceRepository;
            _balanceService = balanceService;
            _resourceRepository = resourceRepository;
            _unitRepository = unitRepository;
        }

        public async Task<Income> AddIncomeAsync(CreateIncomeDto dto)
        {
            await IncomeValidateAsync(dto);

            var income = new Income { Id = dto.Id, Number = dto.Number, Date = dto.Date };

            var balance = new List<Core.Models.Balance>();
            var resoures = new List<IncomeResource>();

            foreach (var item in dto.resources)
            {
                var b = new Core.Models.Balance { Id = Guid.NewGuid(), ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity };
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
            var existingResources = await _incomeResourceRepository.GetIncomeResourceByIncomeIdAsync(dto.Id);

            await IncomeValidateAsync(dto, existingResources);

            oldIncome.Number = dto.Number;
            oldIncome.Date = dto.Date;

            var oldBalance = new List<Core.Models.Balance>();
            var newBalance = new List<Core.Models.Balance>();

            foreach (var item in existingResources)
            {
                oldBalance.Add(new Core.Models.Balance { ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity });
            }

            foreach (var item in dto.resources)
            {
                newBalance.Add(new Core.Models.Balance { ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity });
            }

            await _balanceService.UpdateBalanceAsync(oldBalance, newBalance);

            await _incomeRepository.UpdateIncomeAsync(oldIncome);

            foreach (var item in existingResources)
            {
                await _incomeResourceRepository.RemoveIncomeResourceAsync(item.Id);
            }

            foreach (var item in dto.resources)
            {
                await _incomeResourceRepository.AddIncomeResourceAsync(new IncomeResource { Id = item.Id, IncomeId = oldIncome.Id, ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity });
            }

            return oldIncome;
        }

        public async Task<Income> DeleteIncomeAsync(Guid id)
        {
            var income = await _incomeRepository.GetIncomeByIdAsync(id);

            if (income == null)
                throw new NotFoundException($"Документ поступления с ID {id} не найден");

            var existingResources = await _incomeResourceRepository.GetIncomeResourceByIncomeIdAsync(id);

            var balance = new List<Core.Models.Balance>();
            foreach (var i in existingResources)
            {
                balance.Add(new Core.Models.Balance { ResourceId = i.ResourceId, UnitId = i.UnitId, Quantity = i.Quantity });
            }

            await _balanceService.RemoveBalanceAsync(balance);

            foreach (var item in existingResources)
            {
                await _incomeResourceRepository.RemoveIncomeResourceAsync(item.Id);
            }

            await _incomeRepository.RemoveIncomeAsync(income.Id);

            return income;
        }

        private async Task IncomeValidateAsync(CreateIncomeDto dto)
        {
            var incomes = await _incomeRepository.GetFiltredIncomeDtosAsync(numbers: new List<string> { dto.Number });
            if (incomes != null && incomes.Any())
            {
                throw new ConflictException($"Невозможно создать документ поступления с номером {dto.Number}. Такой уже зарегистрирован в системе");
            }

            foreach (var item in dto.resources)
            {
                var unit = await _unitRepository.GetUnitByIdAsync(item.UnitId);
                var resource = await _resourceRepository.GetResourceByIdAsync(item.ResourceId);

                if (unit == null)
                    throw new NotFoundException($"Единица измерения не зарегистрирована в системе");

                if (resource == null)
                    throw new NotFoundException($"Ресурс не зарегистрирован в системе");

                if (!unit.IsActive)
                    throw new ConflictException($"Единица измерения {unit.Name} Находится в архиве. Её нельзя использовать");

                if (!resource.IsActive)
                    throw new ConflictException($"Ресурс {resource.Name} Находится в архиве. Его нельзя использовать");
            }
        }

        private async Task IncomeValidateAsync(CreateIncomeDto newIncome, IEnumerable<IncomeResource> oldIncomeResources)
        {
            var oldIncome = await _incomeRepository.GetIncomeByIdAsync(newIncome.Id);

            if (oldIncome == null)
            {
                throw new NotFoundException($"Документ поступления с ID {newIncome.Id} не найден");
            }

            if (newIncome.Number != oldIncome.Number)
            {
                var incomes = await _incomeRepository.GetFiltredIncomeDtosAsync(numbers: new List<string> { newIncome.Number });
                if (incomes != null && incomes.Any())
                {
                    throw new ConflictException($"Невозможно создать документ поступления с номером {newIncome.Number}. Такой уже зарегистрирован в системе");
                }
            }

            var ListoldIncomeResources = oldIncomeResources.ToList();

            foreach (var item in newIncome.resources)
            {
                var unit = await _unitRepository.GetUnitByIdAsync(item.UnitId);
                var resource = await _resourceRepository.GetResourceByIdAsync(item.ResourceId);

                if (unit == null)
                    throw new NotFoundException($"Единица измерения не зарегистрирована в системе");

                if (resource == null)
                    throw new NotFoundException($"Ресурс не зарегистрирован в системе");

                if (!unit.IsActive || !resource.IsActive)
                {
                    var oldResource = ListoldIncomeResources.FirstOrDefault(i => i.UnitId == item.UnitId && i.ResourceId == item.ResourceId);

                    if (oldResource != null)
                    {
                        ListoldIncomeResources.Remove(oldResource);
                        continue;
                    }
                }
                
                if (!unit.IsActive)
                {
                    throw new ConflictException($"Единица измерения {unit.Name} Находится в архиве. Её нельзя использовать");
                }

                if (!resource.IsActive)
                {
                    throw new ConflictException($"Ресурс {resource.Name} Находится в архиве. Его нельзя использовать");
                }
            }
        }
    }
}
