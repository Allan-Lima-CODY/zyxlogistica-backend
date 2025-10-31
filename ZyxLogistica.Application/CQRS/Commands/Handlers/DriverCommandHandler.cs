using AutoMapper;
using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Domain.Entities;
using ZyxLogistica.Domain.Interfaces.Services;
using static ZyxLogistica.Domain.Exceptions;

namespace ZyxLogistica.Application.CQRS.Commands.Handlers;

public class DriverCommandHandler(IDriverService _driverService, IMapper _mapper) :
    IRequestHandler<DriverCommand.CreateDriverCommand, DriverDTO.CommandDTO>,
    IRequestHandler<DriverCommand.UpdateDriverCommand, DriverDTO.CommandDTO>,
    IRequestHandler<DriverCommand.ToggleDriverStatusCommand, DriverDTO.CommandDTO>
{
    public async Task<DriverDTO.CommandDTO> Handle(DriverCommand.CreateDriverCommand request, CancellationToken cancellationToken)
    {
        var driver = _mapper.Map<Driver>(request.DriverInput);

        var savedDriver = await _driverService.CreateAsync(driver);
        return _mapper.Map<DriverDTO.CommandDTO>(savedDriver);
    }

    public async Task<DriverDTO.CommandDTO> Handle(DriverCommand.UpdateDriverCommand request, CancellationToken cancellationToken)
    {
        var driver = await _driverService.GetByIdAsync(request.Id) ?? throw new NotFoundException("Motorista não encontrado");

        driver.Update(request.DriverUpdate.Name, request.DriverUpdate.Phone);
        await _driverService.UpdateAsync(driver);

        return _mapper.Map<DriverDTO.CommandDTO>(driver);
    }

    public async Task<DriverDTO.CommandDTO> Handle(DriverCommand.ToggleDriverStatusCommand request, CancellationToken cancellationToken)
    {
        var driver = await _driverService.ToggleStatusAsync(request.Id);
        return _mapper.Map<DriverDTO.CommandDTO>(driver);
    }
}
