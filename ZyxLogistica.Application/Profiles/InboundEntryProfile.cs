using AutoMapper;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Application.Models;
using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Application.Profiles;

public class InboundEntryProfile : Profile
{
    public InboundEntryProfile()
    {
        CreateMap<InboundEntry, InboundEntryDTO.CommandDTO>()
            .ConstructUsing(i => new InboundEntryDTO.CommandDTO(
                i.Id,
                i.Inventory.Id,
                i.Inventory.Description,   
                i.Inventory.Quantity,      
                i.Inventory.Price,         
                i.Inventory.ProductCode,
                i.Reference,
                i.SupplierName,
                i.Observation,
                i.CreatedAt,
                i.UpdatedAt
            ));

        CreateMap<InboundEntryModel.InboundEntryUpdate, InboundEntry>()
            .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.Reference))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.SupplierName))
            .ForMember(dest => dest.Observation, opt => opt.MapFrom(src => src.Observation))
            .ForMember(dest => dest.Inventory, opt => opt.Ignore());
    }
}
