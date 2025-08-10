using Application.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository
{
    public class ShipmentResourceRepository : IShipmentResourceRepository
    {
        public async Task<IEnumerable<ShipmentResource>?> GetShipmentResourcesByUnitIdAsync(Guid unitId)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<ShipmentResource> query = db.ShipmentResources.Where(s => s.UnitId == unitId);
                var shipmentResourses = await query.ToListAsync();

                if (shipmentResourses == null || shipmentResourses.Count() == 0)
                    return null;

                return shipmentResourses;
            }
        }

        public async Task<IEnumerable<ShipmentResource>?> GetShipmentResourcesByResourceIdAsync(Guid resourceId)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<ShipmentResource> query = db.ShipmentResources.Where(s => s.ResourceId == resourceId);
                var shipmentResourses = await query.ToListAsync();

                if (shipmentResourses == null || shipmentResourses.Count() == 0)
                    return null;

                return shipmentResourses;
            }
        }

        public async Task<ShipmentResource?> GetShipmentResourceByIdAsync(Guid id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<ShipmentResource> query = db.ShipmentResources.Where(s => s.Id == id);
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<ShipmentResource>?> GetShipmentResourceByShipmentIdAsync(Guid shipmentId)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<ShipmentResource> query = db.ShipmentResources.Where(i => i.ShipmentId == shipmentId);
                return await query.ToListAsync();
            }
        }

        public async Task<IEnumerable<Resource>?> GetResourcesUsedInShipmentAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var query = db.ShipmentResources
                    .Include(b => b.Resource)
                    .Select(b => b.Resource)
                    .Distinct()
                    .AsQueryable();

                var resources = await query.ToListAsync();

                return resources;
            }
        }

        public async Task<IEnumerable<Unit>?> GetUnitsUsedInShipmentAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var query = db.ShipmentResources
                    .Include(b => b.Unit)
                    .Select(b => b.Unit)
                    .Distinct()
                    .AsQueryable();

                var units = await query.ToListAsync();

                return units;
            }
        }

        public async Task<ShipmentResource> AddShipmentResourceAsync(ShipmentResource shipmentResource)
        {
            if (shipmentResource == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                await db.ShipmentResources.AddAsync(shipmentResource);
                await db.SaveChangesAsync();
            }
            return shipmentResource;
        }

        public async Task<IEnumerable<ShipmentResource>> AddShipmentResourceAsync(IEnumerable<ShipmentResource> shipmentResource)
        {
            if (shipmentResource == null || shipmentResource.Count() == 0) return null;
            using (AppDbContext db = new AppDbContext())
            {
                await db.ShipmentResources.AddRangeAsync(shipmentResource);
                await db.SaveChangesAsync();
            }
            return shipmentResource;
        }

        public async Task<ShipmentResource> UpdateShipmentResourceAsync(ShipmentResource shipmentResource)
        {
            if (shipmentResource == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                db.ShipmentResources.Update(shipmentResource);
                await db.SaveChangesAsync();
            }
            return shipmentResource;
        }

        public async Task<ShipmentResource> RemoveShipmentResourceAsync(Guid Id)
        {
            var shipmentResource = await GetShipmentResourceByIdAsync(Id);
            if (shipmentResource == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                db.ShipmentResources.Remove(shipmentResource);
                await db.SaveChangesAsync();
            }
            return shipmentResource;
        }
    }
}
