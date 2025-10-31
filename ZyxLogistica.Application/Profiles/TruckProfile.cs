using AutoMapper;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Application.Models;
using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Application.Profiles;

public class TruckProfile : Profile
{
    public TruckProfile()
    {
        CreateMap<Truck, TruckDTO.CommandDTO>();
        CreateMap<Truck, TruckDTO.ListDTO>();

        CreateMap<TruckModel.TruckInput, Truck>()
            .ConstructUsing(src => Truck.Create(
                src.LicensePlate,
                src.Model,
                src.Year,
                src.CapacityKg,
                true
            ));
    }
}
