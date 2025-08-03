using Application.Interfaces;
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

        public ShipmentService(IShipmentRepository shipmentRepository, IShipmentResourceRepository shipmentResourceRepository, IBalanceService balanceService) 
        { 
            _shipmentRepository = shipmentRepository;
            _shipmentResourceRepository = shipmentResourceRepository;
            _balanceService = balanceService;
        }

        public async Task<Shipment> AddShipmentAsync(CreateShipmentDto dto)
        {
            var oldShipment = await _shipmentRepository.GetFiltredShipmentDtosAsync(numbers: new List<string> { dto.Number });

            if(oldShipment != null && oldShipment.Count() > 0)
                throw new ConflictException($"Невозможно создать документ отгрузки с номером {dto.Number}. Такой уже зарегистрирован в системе");

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
                var balance = new List<Balance>();

                foreach (var item in existingResources)
                {
                    var b = new Balance { Id = Guid.NewGuid(), ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity };
                    balance.Add(b);
                }

                await _balanceService.RemoveBalanceAsync(balance);
            }
            else
            {
                var balance = new List<Balance>();

                foreach (var item in existingResources)
                {
                    var b = new Balance { Id = Guid.NewGuid(), ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity };
                    balance.Add(b);
                }

                await _balanceService.AddBalanceAsync(balance);
            }

            shipment.IsSigned = ! shipment.IsSigned;
            await _shipmentRepository.UpdateShipmentAsync(shipment);
            return shipment;
        }

        public async Task<Shipment> UpdateShipmentAsync(CreateShipmentDto dto)
        {
            var oldShipment = await _shipmentRepository.GetShipmentByIdAsync(dto.Id);

            if (oldShipment == null)
            {
                throw new NotFoundException($"Документ поступления с ID {dto.Id} не найден");
            }

            if (dto.Number != oldShipment.Number)
            {
                var shipments = await _shipmentRepository.GetFiltredShipmentDtosAsync(numbers: new List<string> { dto.Number });
                if (shipments != null && shipments.Any())
                {
                    throw new ConflictException($"Невозможно создать документ отгрузки с номером {dto.Number}. Такой уже зарегистрирован в системе");
                }
            }

            oldShipment.Number = dto.Number;
            oldShipment.Date = dto.Date;
            oldShipment.ClientId = dto.ClientId;

            var existingResources = await _shipmentResourceRepository.GetShipmentResourceByShipmentIdAsync(dto.Id);

            if (dto.IsSigned != oldShipment.IsSigned)
            {
                var oldBalance = new List<Balance>();
                var newBalance = new List<Balance>();

                foreach (var item in existingResources)
                {
                    oldBalance.Add(new Balance { ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity });
                }

                foreach (var item in dto.resources)
                {
                    newBalance.Add(new Balance { ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity });
                }

                await _balanceService.UpdateBalanceAsync(oldBalance, newBalance);
            }

            oldShipment.IsSigned = dto.IsSigned;
            await _shipmentRepository.UpdateShipmentAsync(oldShipment);

            foreach (var item in existingResources)
            {
                await _shipmentResourceRepository.RemoveShipmentResourceAsync(item.Id);
            }

            foreach (var item in dto.resources)
            {
                await _shipmentResourceRepository.AddShipmentResourceAsync(new ShipmentResource { Id = Guid.NewGuid(), ShipmentId = oldShipment.Id, ResourceId = item.ResourceId, UnitId = item.UnitId, Quantity = item.Quantity });
            }


            return oldShipment;
        }

        public async Task<Shipment> DeleteShipmentAsync(Guid id)
        {
            var shipment = await _shipmentRepository.GetShipmentByIdAsync(id);

            if (shipment == null)
                throw new NotFoundException($"Документ с ID {id} не найден");

            var existingResources = await _shipmentResourceRepository.GetShipmentResourceByShipmentIdAsync(id);

            if (shipment.IsSigned == true)
            {
                var balance = new List<Balance>();
                foreach (var i in existingResources)
                {
                    balance.Add(new Balance { ResourceId = i.ResourceId, UnitId = i.UnitId, Quantity = i.Quantity });
                }

                await _balanceService.RemoveBalanceAsync(balance);
            }
            foreach (var item in existingResources)
            {
                await _shipmentResourceRepository.RemoveShipmentResourceAsync(item.Id);
            }

            await _shipmentRepository.RemoveShipmentAsync(shipment.Id);

            return shipment;
        }
    }
}
