using AutoMapper;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Application.Models;
using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Application.Profiles;

public class ExpeditionProfile : Profile
{
    public ExpeditionProfile()
    {
        CreateMap<Expedition, ExpeditionDTO.CommandDTO>()
            .ConstructUsing(e => new ExpeditionDTO.CommandDTO(
                e.Id,
                e.Order.Id,
                e.Order.OrderNumber,
                e.Order.CustomerName,
                e.Order.Status.ToString(),

                e.Driver.Id,
                e.Driver.Name,

                e.Truck.Id,
                e.Truck.Model,
                e.Truck.LicensePlate,

                e.DeliveryForecast,
                e.Observation,
                e.CreatedAt,
                e.UpdatedAt
            ));

        CreateMap<ExpeditionModel.ExpeditionUpdate, Expedition>()
            .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));
    }
}
