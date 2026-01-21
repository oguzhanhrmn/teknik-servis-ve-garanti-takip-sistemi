using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TSGTS.Business.Mappings;
using TSGTS.Business.Services;
using TSGTS.Core.DTOs;
using TSGTS.Core.Entities;
using TSGTS.Tests.Fakes;
using Xunit;

namespace TSGTS.Tests;

public class TicketManagerTests
{
    private readonly IMapper _mapper;

    public TicketManagerTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task UpdateStatusAsync_ShouldUpdateStatus_AndAddActionLog()
    {
        // Arrange
        var ticketRepo = new FakeGenericRepository<ServiceTicket>();
        var logRepo = new FakeGenericRepository<ActionLog>();
        var manager = new TicketManager(ticketRepo, logRepo, _mapper);

        await ticketRepo.AddAsync(new ServiceTicket
        {
            CustomerId = 1,
            DeviceId = 1,
            OpenedByUserId = 1,
            StatusId = 1,
            CreatedDate = DateTime.UtcNow
        });

        // Act
        var updated = await manager.UpdateStatusAsync(1, statusId: 2, actionLog: "Durum değişti", userId: 99);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal(2, updated!.StatusId);

        var logs = await logRepo.GetAllAsync();
        Assert.Single(logs);
        Assert.Equal(99, logs.First().UserId);
        Assert.Equal("Durum değişti", logs.First().ActionDescription);
    }
}
