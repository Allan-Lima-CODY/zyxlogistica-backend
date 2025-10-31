using AutoMapper;
using MediatR;
using ZyxLogistica.Application.CQRS.Commands;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Interfaces.Repositories;
using ZyxLogistica.Domain.Interfaces.Services;
using static ZyxLogistica.Domain.Exceptions;

namespace ZyxLogistica.Application.CQRS.Commands.Handlers;

public class ExpeditionCommandHandler(IExpeditionService _expeditionService, IOrderRepository _orderRepository, IDriverRepository _driverRepository, ITruckRepository _truckRepository, IMapper _mapper) :
      IRequestHandler<ExpeditionCommand.CreateExpeditionCommand, ExpeditionDTO.CommandDTO>,
      IRequestHandler<ExpeditionCommand.UpdateExpeditionCommand, ExpeditionDTO.CommandDTO>
{
    public async Task<ExpeditionDTO.CommandDTO> Handle(ExpeditionCommand.CreateExpeditionCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetById(request.Input.OrderId) ?? throw new NotFoundException("Pedido não encontrado.");
        var driver = await _driverRepository.GetById(request.Input.DriverId) ?? throw new NotFoundException("Motorista não encontrado.");
        var truck = await _truckRepository.GetById(request.Input.TruckId) ?? throw new NotFoundException("Caminhão não encontrado.");

        var expedition = Expedition.Create(order, driver, truck, request.Input.DeliveryForecast, request.Input.Observation);

        var saved = await _expeditionService.CreateAsync(expedition);
        return _mapper.Map<ExpeditionDTO.CommandDTO>(saved);
    }

    public async Task<ExpeditionDTO.CommandDTO> Handle(ExpeditionCommand.UpdateExpeditionCommand request, CancellationToken cancellationToken)
    {
        var expedition = await _expeditionService.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("Expedição não encontrada.");

        var driver = await _driverRepository.GetById(request.Update.DriverId)
            ?? throw new NotFoundException("Motorista não encontrado.");

        var truck = await _truckRepository.GetById(request.Update.TruckId)
            ?? throw new NotFoundException("Caminhão não encontrado.");

        expedition.Update(
            request.Update.DeliveryForecast,
            request.Update.Observation,
            truck,
            driver
        );

        var updated = await _expeditionService.UpdateAsync(request.Id, expedition, request.OrderStatus);
        return _mapper.Map<ExpeditionDTO.CommandDTO>(updated);
    }
}
