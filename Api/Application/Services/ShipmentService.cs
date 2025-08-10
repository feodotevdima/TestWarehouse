using Application.Interfaces;
using Application.Repository;
using Application.Services;
using Core.Exceptions;
using Core.Models;
using Persistence.Dto;

namespace Application.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IShipmentResourceRepository _shipmentResourceRepository;
        private readonly IBalanceService _balanceService;
        private readonly IUnitRepository _unitRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IClientRepository _clientRepository;

        public ShipmentService(IShipmentRepository shipmentRepository, IShipmentResourceRepository shipmentResourceRepository, IBalanceService balanceService, IUnitRepository unitRepository, IResourceRepository resourceRepository, IClientRepository clientRepository) 
        { 
            _shipmentRepository = shipmentRepository;
            _shipmentResourceRepository = shipmentResourceRepository;
            _balanceService = balanceService;
            _unitRepository = unitRepository;
            _resourceRepository = resourceRepository;
            _clientRepository = clientRepository;
        }

        public async Task<Shipment> AddShipmentAsync(CreateShipmentDto dto)
        {
            await ShipmentValidateAsync(dto);

            var shipment = new Shipment { Id = dto.Id, Number = dto.Number, ClientId = dto.ClientId, Date = dto.Date };   
            await _shipmentRepository.AddShipmentAsync(shipment);

            foreach (var resourse in dto.resources)
            {
                await _shipmentResourceRepository.AddShipmentResourceAsync(new ShipmentResource { Id = resourse.Id, ShipmentId = shipment.Id, UnitId = resourse.UnitId, ResourceId = resourse.ResourceId, Quantity = resourse.Quantity });
            }

            return shipment;
        }

        public async Task<Shipment> ChangeShipmentStatusAsync(Guid id)
        {
            var shipment = await _shipmentRepository.GetShipmentByIdAsync(id);
            if (shipment == null)
            {
                throw new ConflictException($"Такого докумена не существует");
            }

            var existingResources = await _shipmentResourceRepository.GetShipmentResourceByShipmentIdAsync(id);

            if (shipment.IsSigned == false)
            {
                await RemoveBalanceAsync(existingResources);
            }
            else
            {
                await AddBalanceAsync(existingResources);
            }

            shipment.IsSigned = ! shipment.IsSigned;
            await _shipmentRepository.UpdateShipmentAsync(shipment);
            return shipment;
        }

        public async Task<Shipment> UpdateShipmentAsync(CreateShipmentDto dto)
        {
            var oldShipment = await _shipmentRepository.GetShipmentByIdAsync(dto.Id);

            var existingResources = await _shipmentResourceRepository.GetShipmentResourceByShipmentIdAsync(dto.Id);

            await ShipmentValidateAsync(dto, existingResources);

            oldShipment.Number = dto.Number;
            oldShipment.Date = dto.Date;
            oldShipment.ClientId = dto.ClientId;

            var resources = new List<ShipmentResource>();
            foreach (var item in dto.resources)
            {
                var resource = new ShipmentResource { Id = item.Id, ShipmentId = oldShipment.Id, ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity };
                resources.Add(resource);
            }

            if (dto.IsSigned && oldShipment.IsSigned)
            {
                await UpdateBalanceAsync(existingResources, resources);
            }
            else if(dto.IsSigned && !oldShipment.IsSigned)
            {
                await RemoveBalanceAsync(resources);
            }
            else if (!dto.IsSigned && oldShipment.IsSigned)
            {
                await AddBalanceAsync(resources);
            }

            oldShipment.IsSigned = dto.IsSigned;
            await _shipmentRepository.UpdateShipmentAsync(oldShipment);

            foreach (var item in existingResources)
            {
                await _shipmentResourceRepository.RemoveShipmentResourceAsync(item.Id);
            }

            await _shipmentResourceRepository.AddShipmentResourceAsync(resources);

            return oldShipment;
        }

        public async Task<Shipment> DeleteShipmentAsync(Guid id)
        {
            var shipment = await _shipmentRepository.GetShipmentByIdAsync(id);

            if (shipment == null)
                throw new NotFoundException($"Документ с ID {id} не найден");

            var existingResources = await _shipmentResourceRepository.GetShipmentResourceByShipmentIdAsync(id);

            if (shipment.IsSigned)
            {
                await AddBalanceAsync(existingResources);
            }

            foreach (var item in existingResources)
            {
                await _shipmentResourceRepository.RemoveShipmentResourceAsync(item.Id);
            }

            await _shipmentRepository.RemoveShipmentAsync(shipment.Id);

            return shipment;
        }



        private async Task RemoveBalanceAsync(IEnumerable<ShipmentResource> resources)
        {
            var balance = new List<Core.Models.Balance>();

            foreach (var item in resources)
            {
                var b = new Core.Models.Balance { Id = Guid.NewGuid(), ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity };
                balance.Add(b);
            }

            await _balanceService.RemoveBalanceAsync(balance);
        }


        private async Task AddBalanceAsync(IEnumerable<ShipmentResource> resources)
        {
            var balance = new List<Core.Models.Balance>();

            foreach (var item in resources)
            {
                var b = new Core.Models.Balance { Id = Guid.NewGuid(), ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity };
                balance.Add(b);
            }

            await _balanceService.AddBalanceAsync(balance);
        }

        private async Task UpdateBalanceAsync(IEnumerable<ShipmentResource> oldRresources, IEnumerable<ShipmentResource> newRresources)
        {
            var oldBalance = new List<Core.Models.Balance>();
            var newBalance = new List<Core.Models.Balance>();

            foreach (var item in oldRresources)
            {
                oldBalance.Add(new Core.Models.Balance
                {
                    ResourceId = item.ResourceId,
                    UnitId = item.UnitId,
                    Quantity = item.Quantity
                });
            }

            foreach (var item in newRresources)
            {
                newBalance.Add(new Core.Models.Balance
                {
                    ResourceId = item.ResourceId,
                    UnitId = item.UnitId,
                    Quantity = item.Quantity
                });
            }

            await _balanceService.UpdateBalanceAsync(oldBalance, newBalance);
        }

        private async Task ShipmentValidateAsync(CreateShipmentDto dto)
        {
            if(dto.resources.Count == 0)
                throw new BadRequestException($"Невозможно создать документ отгрузки так как он пустой");

            var shipments = await _shipmentRepository.GetFiltredShipmentDtosAsync(numbers: new List<string> { dto.Number });
            if (shipments != null && shipments.Any())
            {
                throw new ConflictException($"Невозможно создать документ отгрузки с номером {dto.Number}. Такой уже зарегистрирован в системе");
            }

            var client = await _clientRepository.GetClientByIdAsync(dto.ClientId);
            if (client == null)
                throw new NotFoundException($"Клиент не зарегистрирована в системе");

            if (!client.IsActive)
                throw new ConflictException($"Клиент {client.Name} Находится в архиве. Её нельзя использовать");

            foreach (var item in dto.resources)
            {
                var unit = await _unitRepository.GetUnitByIdAsync(item.UnitId);
                var resource = await _resourceRepository.GetResourceByIdAsync(item.ResourceId);

                if (unit == null)
                    throw new NotFoundException($"Единица измерения не зарегистрирована в системе");

                if (resource == null)
                    throw new NotFoundException($"Ресурс не зарегистрирован в системе");
            }
        }

        private async Task ShipmentValidateAsync(CreateShipmentDto newShipment, IEnumerable<ShipmentResource> oldShipmentResources)
        {
            var oldShipment = await _shipmentRepository.GetShipmentByIdAsync(newShipment.Id);

            if (oldShipment == null)
            {
                throw new NotFoundException($"Документ отгрузки с ID {newShipment.Id} не найден");
            }

            if (newShipment.Number != oldShipment.Number)
            {
                var shipments = await _shipmentRepository.GetFiltredShipmentDtosAsync(numbers: new List<string> { newShipment.Number });
                if (shipments != null && shipments.Any())
                {
                    throw new ConflictException($"Невозможно создать документ отгрузки с номером {newShipment.Number}. Такой уже зарегистрирован в системе");
                }
            }

            if (newShipment.ClientId != oldShipment.ClientId)
            {
                var client = await _clientRepository.GetClientByIdAsync(newShipment.ClientId);
                if (client == null)
                    throw new NotFoundException($"Клиент не зарегистрирована в системе");

                if (!client.IsActive)
                    throw new ConflictException($"Клиент {client.Name} Находится в архиве. Её нельзя использовать");
            }

            var ListoldShipmentResources = oldShipmentResources.ToList();

            foreach (var item in newShipment.resources)
            {
                var unit = await _unitRepository.GetUnitByIdAsync(item.UnitId);
                var resource = await _resourceRepository.GetResourceByIdAsync(item.ResourceId);

                if (unit == null)
                    throw new NotFoundException($"Единица измерения не зарегистрирована в системе");

                if (resource == null)
                    throw new NotFoundException($"Ресурс не зарегистрирован в системе");
            }
        }
    }
}
