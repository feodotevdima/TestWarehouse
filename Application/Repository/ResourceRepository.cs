using Application.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository
{
    public class ResourceRepository : IResourceRepository
    {
        public async Task<IEnumerable<Resource>> GetAllResourcesAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var resources = await db.Resources.ToListAsync();
                return resources;
            }
        }

        public async Task<IEnumerable<Resource>> GetActiveResourcesAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Resource> query = db.Resources.Where(r => r.IsActive);
                return await query.ToListAsync();
            }
        }

        public async Task<IEnumerable<Resource>> GetArchiveResourcesAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Resource> query = db.Resources.Where(r => !r.IsActive);
                return await query.ToListAsync();
            }
        }

        public async Task<Resource?> GetResourceByIdAsync(Guid id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Resource> query = db.Resources.Where(r => r.Id == id);
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<Resource?> GetResourceByNameAsync(string name)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Resource> query = db.Resources.Where(r => r.Name == name);
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<Resource> AddResourceAsync(Resource resource)
        {
            if (resource == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                await db.Resources.AddAsync(resource);
                await db.SaveChangesAsync();
            }
            return resource;
        }

        public async Task<Resource> UpdateResourceAsync(Resource resource)
        {
            if (resource == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                db.Resources.Update(resource);
                await db.SaveChangesAsync();
            }
            return resource;
        }

        public async Task<Resource> RemoveResourceAsync(Guid Id)
        {
            var resource = await GetResourceByIdAsync(Id);
            if (resource == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                db.Resources.Remove(resource);
                await db.SaveChangesAsync();
            }
            return resource;
        }
    }
}
