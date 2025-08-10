using Application.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Dto;

namespace Application.Repository
{
    public class IncomeRepository: IIncomeRepository
    {
        public async Task<IEnumerable<GetIncomeDto>> GetFiltredIncomeDtosAsync(
            DateOnly? start = null,
            DateOnly? end = null,
            List<string>? numbers = null,
            List<Guid>? resourceIds = null,
            List<Guid>? unitIds = null)
        {
            using (var db = new AppDbContext())
            {
                var incomesQuery = db.Incomes
                    .Include(i => i.IncomeResources)
                        .ThenInclude(ir => ir.Resource)
                    .Include(i => i.IncomeResources)
                        .ThenInclude(ir => ir.Unit)
                    .AsQueryable();

                if (start.HasValue)
                    incomesQuery = incomesQuery.Where(i => i.Date >= start.Value);

                if (end.HasValue)
                    incomesQuery = incomesQuery.Where(i => i.Date <= end.Value);

                if (numbers != null && numbers.Any())
                    incomesQuery = incomesQuery.Where(i => numbers.Contains(i.Number));

                var incomes = await incomesQuery.ToListAsync();

                bool hasResourceFilter = resourceIds != null && resourceIds.Any();
                bool hasUnitFilter = unitIds != null && unitIds.Any();
                bool hasAnyResourceFilter = hasResourceFilter || hasUnitFilter;

                var result = incomes
                    .Select(i => new GetIncomeDto
                    {
                        Id = i.Id,
                        Number = i.Number,
                        Date = i.Date,
                        resources = i.IncomeResources
                            .Where(ir =>
                                (!hasResourceFilter || resourceIds!.Contains(ir.ResourceId)) &&
                                (!hasUnitFilter || unitIds!.Contains(ir.UnitId)))
                            .Select(ir => new IncomeResourceDto
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
                    .Where(dto => !hasAnyResourceFilter || dto.resources.Any())
                    .ToList();

                return result;
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
                    }).FirstOrDefaultAsync();
            }
        }

        public async Task<Income?> GetIncomeByIdAsync(Guid id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                return await db.Incomes.FirstOrDefaultAsync(i => i.Id == id);
            }
        }

        public async Task<List<string>?> GetIncomeNumbersAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                return await db.Incomes.Select(i => i.Number).ToListAsync();
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
