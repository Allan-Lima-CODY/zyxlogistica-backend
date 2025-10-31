using Microsoft.Extensions.DependencyInjection;
using ZyxLogistica.Domain.Interfaces.Repositories;
using ZyxLogistica.Infrastructure.Repositories;

namespace ZyxLogistica.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<ITruckRepository, TruckRepository>();
        services.AddScoped<IExpeditionRepository, ExpeditionRepository>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IInboundEntryRepository, InboundEntryRepository>();

        return services;
    }
}
