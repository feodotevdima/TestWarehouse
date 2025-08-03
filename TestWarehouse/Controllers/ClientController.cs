using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Persistence.Dto;

namespace TestWarehouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IClientService _clientService;

        public ClientController(IClientRepository clientRepository, IClientService clientService)
        {
            _clientRepository = clientRepository;
            _clientService = clientService;
        }

        [HttpGet("all")]
        public async Task<IResult> GetAllClientsAsync()
        {
            var clients = await _clientRepository.GetAllClientsAsync();
            return Results.Ok(clients);
        }

        [HttpGet("active")]
        public async Task<IResult> GetActiveClientsAsync()
        {
            var clients = await _clientRepository.GetActiveClientsAsync();
            return Results.Ok(clients);
        }

        [HttpGet("archive")]
        public async Task<IResult> GetArchiveClientsAsync()
        {
            var clients = await _clientRepository.GetArchiveClientsAsync();
            return Results.Ok(clients);
        }

        [HttpGet("id/{id}")]
        public async Task<IResult> GetClientByIdAsync(Guid id)
        {
            var client = await _clientRepository.GetClientByIdAsync(id);
            return Results.Ok(client);
        }

        [HttpPost]
        public async Task<IResult> AddClientAsync([FromBody] CreateClientDto client)
        {
            var newClient = await _clientService.AddClientAsync(client);
            return Results.Ok(newClient);
        }

        [HttpPut("change-status/{id}")]
        public async Task<IResult> ChangeClientStatusAsync(Guid id)
        {
            var client = await _clientService.ChangeClientStatusAsync(id);
            return Results.Ok(client);
        }

        [HttpPut]
        public async Task<IResult> UpdateClientAsync([FromBody] CreateClientDto client)
        {
            var newClient = await _clientService.UpdateClientAsync(client);
            return Results.Ok(newClient);
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteClientAsync(Guid id)
        {
            var newClient = await _clientService.DeleteClientAsync(id);
            return Results.Ok(newClient);
        }
    }
}
