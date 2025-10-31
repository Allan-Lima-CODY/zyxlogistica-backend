using AutoMapper;
using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Domain.Interfaces.Repositories;
using ZyxLogistica.Domain.Interfaces.Services;

namespace ZyxLogistica.Application.CQRS.Queries.Handlers;

public class DriverQueryHandler(IDriverService _driverService, IDriverRepository _driverRepository, IMapper _mapper) : 
    IRequestHandler<DriverQuery.GetDriversByDateRangeQuery, List<DriverDTO.CommandDTO>?>,
    IRequestHandler<DriverQuery.GetAvailableDriversQuery, List<DriverDTO.ListDTO>?>
{
    public async Task<List<DriverDTO.CommandDTO>?> Handle(DriverQuery.GetDriversByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var drivers = await _driverService.GetDriversByDateRangeAsync(request.StartDate, request.EndDate);

        return _mapper.Map<List<DriverDTO.CommandDTO>?>(drivers);
    }

    public async Task<List<DriverDTO.ListDTO>?> Handle(DriverQuery.GetAvailableDriversQuery request, CancellationToken cancellationToken)
    {
        var result = await _driverRepository.GetAvailableDriversAsync();
        return _mapper.Map<List<DriverDTO.ListDTO>?>(result);
    }
}
