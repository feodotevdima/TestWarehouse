using Application.Interfaces;
using Core.Exceptions;
using Core.Models;

namespace Application.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceRepository _balanceRepository;

        public BalanceService(IBalanceRepository balanceRepository)
        {
            _balanceRepository = balanceRepository;
        }

        public async Task<IEnumerable<Balance>> AddBalanceAsync(IEnumerable<Balance> balance)
        {
            foreach (var item in balance)
            {
                if (item.Quantity == 0)
                    continue;

                var oldBalance = (await _balanceRepository.GetFiltredBalanceAsync(new List<Guid> { item.ResourceId }, new List<Guid> { item.UnitId })).FirstOrDefault();

                if (oldBalance != null)
                {
                    oldBalance.Quantity += item.Quantity;
                    await _balanceRepository.UpdateBalanceAsync(oldBalance);
                }
                else
                {
                    await _balanceRepository.AddBalanceAsync(item);
                }
            }

            return balance;
        }

        public async Task<IEnumerable<Balance>> RemoveBalanceAsync(IEnumerable<Balance> balance)
        {
            foreach (var item in balance)
            {
                if (item.Quantity == 0)
                    continue;

                var oldBalance = (await _balanceRepository.GetFiltredBalanceAsync(new List<Guid> { item.ResourceId }, new List<Guid> { item.UnitId })).FirstOrDefault();

                if (oldBalance == null)
                {
                    throw new UnprocessableEntityException($"Невозможно изменить колличество, на складе не хватает ресурсов");
                }
                else
                {
                    if (oldBalance.Quantity < item.Quantity)
                    {
                        throw new UnprocessableEntityException($"Невозможно изменить колличество, на складе не хватает ресурсов");
                    }
                    else if (oldBalance.Quantity == item.Quantity)
                    {
                        await _balanceRepository.RemoveBalanceAsync(oldBalance.Id);
                    }
                    else
                    {
                        oldBalance.Quantity -= item.Quantity;
                        await _balanceRepository.UpdateBalanceAsync(oldBalance);
                    }
                }
            }
            return balance;
        }


        public async Task<IEnumerable<Balance>> UpdateBalanceAsync(IEnumerable<Balance> oldBalance, IEnumerable<Balance> newBalance)
        {
            var currentBalances = (await _balanceRepository.GetFiltredBalanceAsync()).ToList();

            foreach (var item in oldBalance)
            {
                var existingBalance = currentBalances.FirstOrDefault(b =>
                    b.ResourceId == item.ResourceId &&
                    b.UnitId == item.UnitId);

                if (existingBalance == null)
                {
                    throw new UnprocessableEntityException($"Невозможно изменить колличество, на складе не хватает ресурсов");
                }

                existingBalance.Quantity -= item.Quantity;
            }

            foreach (var item in newBalance)
            {
                var existingBalance = currentBalances.FirstOrDefault(b =>
                    b.ResourceId == item.ResourceId &&
                    b.UnitId == item.UnitId);

                if (existingBalance == null)
                {
                    var newBalanceItem = new Balance
                    {
                        Id = Guid.NewGuid(),
                        ResourceId = item.ResourceId,
                        UnitId = item.UnitId,
                        Quantity = item.Quantity
                    };
                    currentBalances.Add(newBalanceItem);
                    await _balanceRepository.AddBalanceAsync(newBalanceItem);
                }
                else
                {
                    existingBalance.Quantity += item.Quantity;
                }
            }

            foreach (var b in currentBalances)
            {
                if (b.Quantity < 0)
                {
                    throw new UnprocessableEntityException($"Невозможно изменить колличество, на складе не хватает ресурсов");
                }
            }

            foreach (var b in currentBalances.ToList())
            {
                if (b.Quantity == 0)
                {
                    currentBalances.Remove(b);
                    await _balanceRepository.RemoveBalanceAsync(b.Id);
                }
                else
                {
                    await _balanceRepository.UpdateBalanceAsync(b);
                }
            }
            return currentBalances;
        }
    }
}
