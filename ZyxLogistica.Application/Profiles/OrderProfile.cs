using AutoMapper;
using ZyxLogistica.Application.DTOs;
using ZyxLogistica.Application.Models;
using ZyxLogistica.Domain.Entities;

namespace ZyxLogistica.Application.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDTO.CommandDTO>();
        CreateMap<Order, OrderDTO.ListDTO>();

        CreateMap<OrderInventory, OrderDTO.OrderInventoryDTO>()
            .ForMember(dest => dest.InventoryId, opt => opt.MapFrom(src => src.Inventory.Id))
            .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.Inventory.ProductCode))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Inventory.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Inventory.Price));

        CreateMap<OrderModel.OrderInput, Order>()
            .ConstructUsing(src => Order.Create(src.OrderNumber, src.CustomerName));

        CreateMap<OrderModel.OrderInventoryInput, OrderInventory>()
            .ForMember(dest => dest.Inventory, opt => opt.Ignore());
    }
}