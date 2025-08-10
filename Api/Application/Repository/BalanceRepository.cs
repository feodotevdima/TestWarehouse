using Application.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Dto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Repository
{
    public class BalanceRepository : IBalanceRepository
    {
        public async Task<IEnumerable<GetBalanceDto>> GetFiltredBalanceDtoAsync(List<Guid>? resourceIds = null, List<Guid>? unitIds = null)
        {
            using (AppDbContext db = new AppDbContext())
            {
                var query = db.Balances
                    .Include(i => i.Resource)
                    .Include(i => i.Unit)
                    .AsQueryable();

                if (resourceIds != null && resourceIds.Any())
                {
                    query = query.Where(b => resourceIds.Contains(b.ResourceId));
                }

                if (unitIds != null && unitIds.Any())
                {
                    query = query.Where(b => unitIds.Contains(b.UnitId));
                }

                var balances = await query.ToListAsync();

                return balances.Select(b => new GetBalanceDto
                {
                    Id = b.Id,
                    ResourceId = b.ResourceId,
                    ResourceName = b.Resource.Name,
                    UnitId = b.UnitId,
                    UnitName = b.Unit.Name,
                    Quantity = b.Quantity,
                }).ToList();
            }
        }

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

        public async Task<IEnumerable<Resource>?> GetResourcesUsedInBalanceAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var query = db.Balances
                    .Include(b => b.Resource)
                    .Select(b => b.Resource)
                    .Distinct()
                    .AsQueryable();

                var resources = await query.ToListAsync();

                return resources;
            }
        }

        public async Task<IEnumerable<Unit>?> GetUnitsUsedInBalanceAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var query = db.Balances
                    .Include(b => b.Unit)
                    .Select(b => b.Unit)
                    .Distinct()
                    .AsQueryable();

                var units = await query.ToListAsync();

                return units;
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
