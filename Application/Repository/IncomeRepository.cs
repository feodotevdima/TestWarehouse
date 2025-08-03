using Application.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Dto;

namespace Application.Repository
{
    public class IncomeRepository: IIncomeRepository
    {
        public async Task<IEnumerable<GetIncomeDto>> GetFiltredIncomeDtosAsync(DateTime? start = null, DateTime? end = null, List<string>? numbers = null, List<Guid>? resourceIds = null, List<Guid>? unitIds = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Incomes
                    .Include(i => i.IncomeResources)
                    .ThenInclude(ir => ir.Resource)
                    .Include(i => i.IncomeResources)
                    .ThenInclude(ir => ir.Unit)
                    .AsQueryable();

                if (start.HasValue)
                    query = query.Where(i => i.Date >= start.Value);

                if (end.HasValue)
                    query = query.Where(i => i.Date <= end.Value);

                if (numbers != null && numbers.Any())
                    query = query.Where(i => numbers.Contains(i.Number));

                if (resourceIds != null && resourceIds.Any())
                    query = query.Where(i => i.IncomeResources.Any(ir => resourceIds.Contains(ir.ResourceId)));

                if (unitIds != null && unitIds.Any())
                    query = query.Where(i => i.IncomeResources.Any(ir => unitIds.Contains(ir.UnitId)));

                var incomes = await query.ToListAsync();

                return incomes.Select(i => new GetIncomeDto
                {
                    Id = i.Id,
                    Number = i.Number,
                    Date = i.Date,
                    resources = i.IncomeResources.Select(ir => new IncomeResourceDto
                    {
                        Id = ir.Id,
                        IncomeId = ir.IncomeId,
                        ResourceId = ir.ResourceId,
                        ResourceName = ir.Resource.Name,
                        UnitId = ir.UnitId,
                        UnitName = ir.Unit.Name, 
                        Quantity = ir.Quantity
                    }).ToList()
                }).ToList();
            }
        }

        public async Task<GetIncomeDto?> GetIncomeDtoByIdAsync(Guid id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                return await db.Incomes
                    .Include(i => i.IncomeResources)
                    .ThenInclude(ir => ir.Resource)
                    .Include(i => i.IncomeResources)
                    .ThenInclude(ir => ir.Unit)
                    .Where(i => i.Id == id)
                    .Select(i => new GetIncomeDto
                    {
                        Id = i.Id,
                        Number = i.Number,
                        Date = i.Date,
                        resources = i.IncomeResources.Select(ir => new IncomeResourceDto
                        {
                            Id = ir.Id,
                            IncomeId = ir.IncomeId,
                            ResourceId = ir.ResourceId,
                            ResourceName = ir.Resource.Name,
                            UnitId = ir.UnitId,
                            UnitName = ir.Unit.Name,
                            Quantity = ir.Quantity
                        }).ToList()
                    })
            .FirstOrDefaultAsync();
            }
        }

        public async Task<Income?> GetIncomeByIdAsync(Guid id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                return db.Incomes.FirstOrDefault(i => i.Id == id);
            }
        }

        public async Task<Income> AddIncomeAsync(Income income)
        {
            if (income == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                await db.Incomes.AddAsync(income);
                await db.SaveChangesAsync();
            }
            return income;
        }

        public async Task<Income> UpdateIncomeAsync(Income income)
        {
            if (income == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                db.Incomes.Update(income);
                await db.SaveChangesAsync();
            }
            return income;
        }

        public async Task<Income> RemoveIncomeAsync(Guid Id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                var income = await db.Incomes.Where(i => i.Id == Id).FirstOrDefaultAsync();
                if (income == null) return null;
                db.Incomes.Remove(income);
                await db.SaveChangesAsync();
                return income;
            }
        }
    }
}
