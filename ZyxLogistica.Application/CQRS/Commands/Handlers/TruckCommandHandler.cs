using AutoMapper;
using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Interfaces.Services;
using static ZyxLogistica.Domain.Exceptions;

namespace ZyxLogistica.Application.CQRS.Commands.Handlers;

public class TruckCommandHandler(ITruckService _truckService, IMapper _mapper) :
    IRequestHandler<TruckCommand.CreateTruckCommand, TruckDTO.CommandDTO>,
    IRequestHandler<TruckCommand.UpdateTruckCommand, TruckDTO.CommandDTO>
{
    public async Task<TruckDTO.CommandDTO> Handle(TruckCommand.CreateTruckCommand request, CancellationToken cancellationToken)
    {
        var truck = _mapper.Map<Truck>(request.TruckInput);

        var savedTruck = await _truckService.CreateAsync(truck);
        return _mapper.Map<TruckDTO.CommandDTO>(savedTruck);
    }

    public async Task<TruckDTO.CommandDTO> Handle(TruckCommand.UpdateTruckCommand request, CancellationToken cancellationToken)
    {
        var truck = await _truckService.GetByIdAsync(request.Id) ?? throw new NotFoundException("Caminhão não encontrado");

        truck.Update(request.TruckUpdate.Model, request.TruckUpdate.Year, request.TruckUpdate.CapacityKg);
        await _truckService.UpdateAsync(truck);

        return _mapper.Map<TruckDTO.CommandDTO>(truck);
    }
}
