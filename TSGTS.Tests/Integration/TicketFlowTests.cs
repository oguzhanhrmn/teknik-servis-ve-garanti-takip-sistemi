using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TSGTS.Business;
using TSGTS.Business.Mappings;
using TSGTS.Core.DTOs;
using TSGTS.DataAccess;
using TSGTS.DataAccess.Repositories;
using Xunit;

namespace TSGTS.Tests.Integration;

public class TicketFlowTests
{
    [Fact]
    public async Task HappyPath_Ticket_Create_UpdateStatus()
    {
        var services = new ServiceCollection();
        services.AddDbContext<TsgtsDbContext>(_ => _.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        services.AddBusinessServices();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        var provider = services.BuildServiceProvider();
        var ctx = provider.GetRequiredService<TsgtsDbContext>();
        var ticketService = provider.GetRequiredService<TSGTS.Business.Interfaces.ITicketService>();
        var customerService = provider.GetRequiredService<TSGTS.Business.Interfaces.ICustomerService>();
        var deviceService = provider.GetRequiredService<TSGTS.Business.Interfaces.IDeviceService>();
        var statusService = provider.GetRequiredService<TSGTS.Business.Interfaces.ITicketStatusService>();

        // seed status
        ctx.TicketStatuses.AddRange(
            new TSGTS.Core.Entities.TicketStatus { StatusName = "Beklemede", ColorCode = "#f1c40f" },
            new TSGTS.Core.Entities.TicketStatus { StatusName = "İşlemde", ColorCode = "#3498db" });
        await ctx.SaveChangesAsync();

        // seed customer/device
        var cust = await customerService.CreateAsync(new CustomerCreateDto
        {
            FirstName = "Test",
            LastName = "Müşteri",
            Phone = "5551234567",
            Email = "test@example.com"
        });
        var dev = await deviceService.CreateAsync(new DeviceCreateDto
        {
            SerialNumber = "SN-TEST-001",
            BrandId = 1,
            ModelId = 1
        });

        // create ticket
        var ticket = await ticketService.CreateAsync(new ServiceTicketCreateDto
        {
            CustomerId = cust.Id,
            DeviceId = dev.Id,
            OpenedByUserId = 1,
            StatusId = 1,
            Description = "Test kayıt"
        });

        Assert.NotNull(ticket);
        Assert.Equal(1, ticket.StatusId);

        // update status
        var updated = await ticketService.UpdateStatusAsync(ticket.Id, 2, "İşlemde", 1);
        Assert.NotNull(updated);
        Assert.Equal(2, updated!.StatusId);
    }
}
