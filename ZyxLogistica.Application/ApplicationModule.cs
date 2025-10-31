using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ZyxLogistica.Application.Services;
using ZyxLogistica.Domain.Interfaces.Services;

namespace ZyxLogistica.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddAutoMapper(cfg => { }, assembly);

        services.AddScoped<IDriverService, DriverService>();
        services.AddScoped<ITruckService, TruckService>();
        services.AddScoped<IExpeditionService, ExpeditionService>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IInboundEntryService, InboundEntryService>();

        return services;
    }
}
