using AutoMapper;
using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Domain.Interfaces.Repositories;
using ZyxLogistica.Domain.Interfaces.Services;

namespace ZyxLogistica.Application.CQRS.Queries.Handlers;

public class TruckQueryHandler(ITruckService _truckService, ITruckRepository _truckRepository, IMapper _mapper) :
    IRequestHandler<TruckQuery.GetTrucksByDateRangeQuery, List<TruckDTO.CommandDTO>?>,
    IRequestHandler<TruckQuery.GetAvailableTrucksQuery, List<TruckDTO.ListDTO>?>
{
    public async Task<List<TruckDTO.CommandDTO>?> Handle(TruckQuery.GetTrucksByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var trucks = await _truckService.GetTrucksByDateRangeAsync(request.StartDate, request.EndDate);

        return _mapper.Map<List<TruckDTO.CommandDTO>?>(trucks);
    }

    public async Task<List<TruckDTO.ListDTO>?> Handle(TruckQuery.GetAvailableTrucksQuery request, CancellationToken cancellationToken)
    {
        var result = await _truckRepository.GetAvailableTrucksAsync();
        return _mapper.Map<List<TruckDTO.ListDTO>>(result);
    }
}