using AutoMapper;
using MediatR;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Domain.Interfaces.Services;

namespace ZyxLogistica.Application.CQRS.Queries.Handlers;

public class InventoryQueryHandler(IInventoryService _inventoryService, IMapper _mapper) :
    IRequestHandler<InventoryQuery.GetInventoriesByDateRangeQuery, List<InventoryDTO.CommandDTO>?>,
    IRequestHandler<InventoryQuery.GetAvailableInventoriesQuery, List<InventoryDTO.CommandDTO>?>
{
    public async Task<List<InventoryDTO.CommandDTO>?> Handle(InventoryQuery.GetInventoriesByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var invs = await _inventoryService.GetInventoriesByDateRangeAsync(request.StartDate, request.EndDate);
        return _mapper.Map<List<InventoryDTO.CommandDTO>?>(invs);
    }

    public async Task<List<InventoryDTO.CommandDTO>?> Handle(InventoryQuery.GetAvailableInventoriesQuery request, CancellationToken cancellationToken)
    {
        var invs = await _inventoryService.GetAvailableInventoriesAsync();
        return _mapper.Map<List<InventoryDTO.CommandDTO>?>(invs);
    }
}
