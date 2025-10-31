using AutoMapper;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Application.Models;
using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Application.Profiles;

public class InventoryProfile : Profile
{
    public InventoryProfile()
    {
        CreateMap<Inventory, InventoryDTO.CommandDTO>();

        CreateMap<InventoryModel.InventoryInput, Inventory>()
            .ConstructUsing(src => Inventory.Create(
                src.ProductCode,
                src.Description,
                src.Quantity,
                src.Price
            ));
    }
}
