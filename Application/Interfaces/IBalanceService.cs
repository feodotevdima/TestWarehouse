using Core.Models;

namespace Application.Interfaces
{
    public interface IBalanceService
    {
        Task<IEnumerable<Balance>> AddBalanceAsync(IEnumerable<Balance> balance);
        Task<IEnumerable<Balance>> RemoveBalanceAsync(IEnumerable<Balance> balance);
        Task<IEnumerable<Balance>> UpdateBalanceAsync(IEnumerable<Balance> oldBalance, IEnumerable<Balance> newBalance);
    }
}