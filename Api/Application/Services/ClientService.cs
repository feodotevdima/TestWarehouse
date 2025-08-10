using Application.Interfaces;
using Core.Exceptions;
using Core.Models;
using Persistence.Dto;

namespace Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IShipmentRepository _shipmentRepository;

        public ClientService(IClientRepository clientRepository, IShipmentRepository shipmentRepository) 
        {
            _clientRepository = clientRepository;
            _shipmentRepository = shipmentRepository;
        }

        public async Task<Client> ChangeClientStatusAsync(Guid id)
        {
            var client = await _clientRepository.GetClientByIdAsync(id);
            client.IsActive = !client.IsActive;
            await _clientRepository.UpdateClientAsync(client);
            return client;
        }

        public async Task<Client> AddClientAsync(CreateClientDto dto)
        {
            var client = await _clientRepository.GetClientByNameAsync(dto.Name);
            if (client != null)
            {
                throw new ConflictException($"Невозможно создать клиента с именем {dto.Name}. Такой уже зарегистрирован в системе");
            }

            client = new Client { Id =  dto.Id, Name = dto.Name, Address = dto.Address };
            await _clientRepository.AddClientAsync(client);
            return client;
        }

        public async Task<Client> UpdateClientAsync(CreateClientDto dto)
        {
            var client = await _clientRepository.GetClientByIdAsync(dto.Id);

            if (client.Name != dto.Name)
            {
                if (await _clientRepository.GetClientByNameAsync(dto.Name) != null)
                {
                    throw new ConflictException($"Невозможно создать клиента с именем {dto.Name}. Такой уже зарегистрирован в системе");
                }
            }

            var newClient = new Client { Id = dto.Id, Name = dto.Name, Address = dto.Address, IsActive = dto.IsActive };
            await _clientRepository.UpdateClientAsync(newClient);
            return newClient;
        }

        public async Task<Client> DeleteClientAsync(Guid id)
        {
            if(await CheckClientToUse(id))
                throw new ConflictException($"Невозможно удалить клиента, так как он используется в системе");

            var client = await _clientRepository.RemoveClientAsync(id);
            return client;
        }

        public async Task<bool> CheckClientToUse( Guid id)
        {
            var shipments = await _shipmentRepository.GetShipmentsByClientIdAsync(id);

            return shipments != null;
        }
    }
}
