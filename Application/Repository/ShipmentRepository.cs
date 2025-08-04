using Application.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Dto;

namespace Application.Repository
{
    public class ShipmentRepository : IShipmentRepository
    {
        public async Task<IEnumerable<GetShipmentDto>> GetFiltredShipmentDtosAsync(DateOnly? start = null, DateOnly? end = null, List<string>? numbers = null, List<Guid>? clientIds = null, List<Guid>? resourceIds = null, List<Guid>? unitIds = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Shipments
                    .Include(i => i.Client)
                    .Include(i => i.ShipmentResources)
                    .ThenInclude(ir => ir.Resource)
                    .Include(i => i.ShipmentResources)
                    .ThenInclude(ir => ir.Unit)
                    .AsQueryable();

                if (start.HasValue)
                    query = query.Where(i => i.Date >= start.Value);

                if (end.HasValue)
                    query = query.Where(i => i.Date <= end.Value);

                if (numbers != null && numbers.Any())
                    query = query.Where(i => numbers.Contains(i.Number));

                if (clientIds != null && clientIds.Any())
                    query = query.Where(i => clientIds.Contains(i.Client.Id));

                if (resourceIds != null && resourceIds.Any())
                    query = query.Where(i => i.ShipmentResources.Any(ir => resourceIds.Contains(ir.ResourceId)));

                if (unitIds != null && unitIds.Any())
                    query = query.Where(i => i.ShipmentResources.Any(ir => unitIds.Contains(ir.UnitId)));

                var shipments = await query.ToListAsync();

                return shipments.Select(i => new GetShipmentDto
                {
                    Id = i.Id,
                    Number = i.Number,
                    ClientId = i.ClientId,
                    ClientName = i.Client.Name,
                    Date = i.Date,
                    IsSigned = i.IsSigned,
                    resources = i.ShipmentResources.Select(ir => new ShipmentResourceDto
                    {
                        Id = ir.Id,
                        ShipmentId = ir.ShipmentId,
                        ResourceId = ir.ResourceId,
                        ResourceName = ir.Resource.Name,
                        UnitId = ir.UnitId,
                        UnitName = ir.Unit.Name,
                        Quantity = ir.Quantity
                    }).ToList()
                }).ToList();
            }
        }

        public async Task<GetShipmentDto?> GetShipmentDtoByIdAsync(Guid id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                return await db.Shipments
                    .Include(i => i.Client)
                    .Include(i => i.ShipmentResources)
                    .ThenInclude(ir => ir.Resource)
                    .Include(i => i.ShipmentResources)
                    .ThenInclude(ir => ir.Unit)
                    .Where(i => i.Id == id)
                    .Select(i => new GetShipmentDto
                    {
                        Id = i.Id,
                        Number = i.Number,
                        ClientId = i.ClientId,
                        ClientName = i.Client.Name,
                        Date = i.Date,
                        IsSigned = i.IsSigned,
                        resources = i.ShipmentResources.Select(ir => new ShipmentResourceDto
                        {
                            Id = ir.Id,
                            ShipmentId = ir.ShipmentId,
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

        public async Task<Shipment?> GetShipmentByIdAsync(Guid id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                return db.Shipments.FirstOrDefault(i => i.Id == id);
            }
        }

        public async Task<IEnumerable<Shipment>?> GetShipmentsByClientIdAsync(Guid clientId)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Shipment> query = db.Shipments.Where(s => s.ClientId == clientId);
                var shipments = await query.ToListAsync();

                if (shipments == null || shipments.Count() == 0)
                    return null;

                return shipments;
            }
        }

        public async Task<Shipment> AddShipmentAsync(Shipment shipment)
        {
            if (shipment == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                await db.Shipments.AddAsync(shipment);
                await db.SaveChangesAsync();
            }
            return shipment;
        }

        public async Task<Shipment> UpdateShipmentAsync(Shipment shipment)
        {
            if (shipment == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                db.Shipments.Update(shipment);
                await db.SaveChangesAsync();
            }
            return shipment;
        }

        public async Task<Shipment> RemoveShipmentAsync(Guid Id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                var shipment = await db.Shipments.Where(i => i.Id == Id).FirstOrDefaultAsync();
                if (shipment == null) return null;
                db.Shipments.Remove(shipment);
                await db.SaveChangesAsync();
                return shipment;
            }
        }
    }
}
