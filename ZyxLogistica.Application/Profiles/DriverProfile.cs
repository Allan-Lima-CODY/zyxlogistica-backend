using AutoMapper;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Application.Models;
using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Application.Profiles;

public class DriverProfile : Profile
{
    public DriverProfile()
    {
        CreateMap<Driver, DriverDTO.CommandDTO>();
        CreateMap<Driver, DriverDTO.ListDTO>();

        CreateMap<DriverModel.DriverInput, Driver>()
            .ConstructUsing(src => Driver.Create(
                src.Name,
                src.Phone,
                src.Cnh,
                src.CnhCategory,
                true
            ));
    }
}