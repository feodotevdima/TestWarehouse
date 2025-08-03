using Application.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository
{
    public class BalanceRepository : IBalanceRepository
    {
        public async Task<IEnumerable<Balance>> GetFiltredBalanceAsync(List<Guid>? resourceIds = null, List<Guid>? unitIds = null)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Balance> query = db.Balances;

                if (resourceIds != null && resourceIds.Any())
                {
                    query = query.Where(b => resourceIds.Contains(b.ResourceId));
                }

                if (unitIds != null && unitIds.Any())
                {
                    query = query.Where(b => unitIds.Contains(b.UnitId));
                }

                return await query.ToListAsync();
            }
        }

        public async Task<Balance?> GetBalanceByIdAsync(Guid id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Balance> query = db.Balances.Where(b => b.Id == id);
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<Balance> AddBalanceAsync(Balance balance)
        {
            if (balance == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                await db.Balances.AddAsync(balance);
                await db.SaveChangesAsync();
            }
            return balance;
        }

        public async Task<Balance> UpdateBalanceAsync(Balance balance)
        {
            if (balance == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                db.Balances.Update(balance);
                await db.SaveChangesAsync();
            }
            return balance;
        }

        public async Task<Balance> RemoveBalanceAsync(Guid Id)
        {
            var balance = await GetBalanceByIdAsync(Id);
            if (balance == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                db.Balances.Remove(balance);
                await db.SaveChangesAsync();
            }
            return balance;
        }
    }
}
