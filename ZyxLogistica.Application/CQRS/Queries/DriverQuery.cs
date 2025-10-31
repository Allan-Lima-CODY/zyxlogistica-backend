using MediatR;
using ZyxLogistica.Application.DTOs;
using static ZyxLogistica.Application.DTOs.DriverDTO;

namespace ZyxLogistica.Application.CQRS.Queries;

public class DriverQuery
{
    public record GetDriversByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<List<CommandDTO>?>;
    public record GetAvailableDriversQuery() : IRequest<List<ListDTO>?>;
}
