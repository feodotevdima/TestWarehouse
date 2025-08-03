using Application.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository
{
    public class UnitRepository : IUnitRepository
    {
        public async Task<IEnumerable<Unit>> GetAllUnitsAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var units = await db.Units.ToListAsync();
                return units;
            }
        }

        public async Task<IEnumerable<Unit>> GetActiveUnitsAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Unit> query = db.Units.Where(u => u.IsActive);
                return await query.ToListAsync();
            }
        }

        public async Task<IEnumerable<Unit>> GetArchiveUnitsAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Unit> query = db.Units.Where(u => !u.IsActive);
                return await query.ToListAsync();
            }
        }

        public async Task<Unit?> GetUnitByIdAsync(Guid id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Unit> query = db.Units.Where(u => u.Id == id);
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<Unit?> GetUnitByNameAsync(string name)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Unit> query = db.Units.Where(u => u.Name == name);
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<Unit> AddUnitAsync(Unit unit)
        {
            if (unit == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                await db.Units.AddAsync(unit);
                await db.SaveChangesAsync();
            }
            return unit;
        }

        public async Task<Unit> UpdateUnitAsync(Unit unit)
        {
            if (unit == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                db.Units.Update(unit);
                await db.SaveChangesAsync();
            }
            return unit;
        }

        public async Task<Unit> RemoveUnitAsync(Guid Id)
        {
            var unit = await GetUnitByIdAsync(Id);
            if (unit == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                db.Units.Remove(unit);
                await db.SaveChangesAsync();
            }
            return unit;
        }
    }
}
