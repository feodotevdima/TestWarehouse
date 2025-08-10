using Core.Models;

namespace Application.Interfaces
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<IEnumerable<Client>> GetActiveClientsAsync();
        Task<IEnumerable<Client>> GetArchiveClientsAsync();
        Task<Client?> GetClientByIdAsync(Guid id);
        Task<Client?> GetClientByNameAsync(string name);
        Task<Client> AddClientAsync(Client client);
        Task<Client> UpdateClientAsync(Client client);
        Task<Client> RemoveClientAsync(Guid Id);
    } 
}
