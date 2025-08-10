using Application.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository
{
    public class IncomeResourceRepository : IIncomeResourceRepository
    {
        public async Task<IEnumerable<IncomeResource>?> GetIncomeResourcesByUnitIdAsync(Guid unitId)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<IncomeResource> query = db.IncomeResources.Where(s => s.UnitId == unitId);
                var incomeResourses = await query.ToListAsync();

                if (incomeResourses == null || incomeResourses.Count() == 0)
                    return null;

                return incomeResourses;
            }
        }

        public async Task<IEnumerable<IncomeResource>?> GetIncomeResourcesByResourceIdAsync(Guid resourceId)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<IncomeResource> query = db.IncomeResources.Where(s => s.ResourceId == resourceId);
                var incomeResource = await query.ToListAsync();

                if (incomeResource == null || incomeResource.Count() == 0)
                    return null;

                return incomeResource;
            }
        }

        public async Task<IEnumerable<IncomeResource>?> GetIncomeResourceByIncomeIdAsync(Guid incomeId)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<IncomeResource> query = db.IncomeResources.Where(i => i.IncomeId == incomeId);
                return await query.ToListAsync();
            }
        }

        public async Task<IncomeResource?> GetIncomeResourceByIdAsync(Guid id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<IncomeResource> query = db.IncomeResources.Where(i => i.Id == id);
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<Resource>?> GetResourcesUsedInIncomeAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var query = db.IncomeResources
                    .Include(b => b.Resource)
                    .Select(b => b.Resource)
                    .Distinct()
                    .AsQueryable();

                var resources = await query.ToListAsync();

                return resources;
            }
        }

        public async Task<IEnumerable<Unit>?> GetUnitsUsedInIncomeAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var query = db.IncomeResources
                    .Include(b => b.Unit)
                    .Select(b => b.Unit)
                    .Distinct()
                    .AsQueryable();

                var units = await query.ToListAsync();

                return units;
            }
        }

        public async Task<IncomeResource> AddIncomeResourceAsync(IncomeResource incomeResource)
        {
            if (incomeResource == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                await db.IncomeResources.AddAsync(incomeResource);
                await db.SaveChangesAsync();
            }
            return incomeResource;
        }

        public async Task<IEnumerable<IncomeResource>> AddIncomeResourceAsync(IEnumerable<IncomeResource> incomeResources)
        {
            if (incomeResources == null || incomeResources.Count() == 0) return null;
            using (AppDbContext db = new AppDbContext())
            {
                await db.IncomeResources.AddRangeAsync(incomeResources);
                await db.SaveChangesAsync();
            }
            return incomeResources;
        }

        public async Task<IncomeResource> UpdateIncomeResourceAsync(IncomeResource incomeResource)
        {
            if (incomeResource == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                db.IncomeResources.Update(incomeResource);
                await db.SaveChangesAsync();
            }
            return incomeResource;
        }

        public async Task<IncomeResource> RemoveIncomeResourceAsync(Guid Id)
        {
            var incomeResource = await GetIncomeResourceByIdAsync(Id);
            if (incomeResource == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                db.IncomeResources.Remove(incomeResource);
                await db.SaveChangesAsync();
            }
            return incomeResource;
        }
    }
}
