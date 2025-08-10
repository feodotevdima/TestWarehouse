using Application.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository
{
    public class ClientRepository: IClientRepository
    {
        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var clients = await db.Clients.ToListAsync();
                return clients;
            }
        }

        public async Task<IEnumerable<Client>> GetActiveClientsAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Client> query = db.Clients.Where(c => c.IsActive);
                return await query.ToListAsync();
            }
        }

        public async Task<IEnumerable<Client>> GetArchiveClientsAsync()
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Client> query = db.Clients.Where(c => !c.IsActive);
                return await query.ToListAsync();
            }
        }

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Client> query = db.Clients.Where(c => c.Id == id);
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<Client?> GetClientByNameAsync(string name)
        {
            using (AppDbContext db = new AppDbContext())
            {
                IQueryable<Client> query = db.Clients.Where(c => c.Name == name);
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<Client> AddClientAsync(Client client)
        {
            if (client == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                await db.Clients.AddAsync(client);
                await db.SaveChangesAsync();
            }
            return client;
        }

        public async Task<Client> UpdateClientAsync(Client client)
        {
            if(client == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                db.Clients.Update(client);
                await db.SaveChangesAsync();
            }
            return client;
        }

        public async Task<Client> RemoveClientAsync(Guid Id)
        {
            var client = await GetClientByIdAsync(Id);
            if (client == null) return null;
            using (AppDbContext db = new AppDbContext())
            {
                db.Clients.Remove(client);
                await db.SaveChangesAsync();
            }
            return client;
        }
    } 
}
