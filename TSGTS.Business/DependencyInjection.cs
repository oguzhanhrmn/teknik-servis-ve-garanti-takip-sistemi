using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TSGTS.Business.Interfaces;
using TSGTS.Business.Mappings;
using TSGTS.Business.Services;
using TSGTS.Business.Validation;

namespace TSGTS.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        services.AddValidatorsFromAssembly(typeof(CustomerCreateDtoValidator).Assembly);

        services.AddScoped<ICustomerService, CustomerManager>();
        services.AddScoped<IDeviceService, DeviceManager>();
        services.AddScoped<ITicketService, TicketManager>();
        services.AddScoped<ITicketStatusService, TicketStatusManager>();
        services.AddScoped<ISparePartService, SparePartManager>();

        return services;
    }
}
