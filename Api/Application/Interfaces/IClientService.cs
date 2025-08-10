using Core.Models;
using Persistence.Dto;

namespace Application.Interfaces
{
    public interface IClientService
    {
        Task<Client> ChangeClientStatusAsync(Guid id);
        Task<Client> AddClientAsync(CreateClientDto dto);
        Task<Client> UpdateClientAsync(CreateClientDto dto);
        Task<Client> DeleteClientAsync(Guid id);
    }
}
