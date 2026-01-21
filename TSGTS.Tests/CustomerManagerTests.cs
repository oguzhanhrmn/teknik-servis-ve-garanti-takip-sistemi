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

public class CustomerManagerTests
{
    private readonly IMapper _mapper;

    public CustomerManagerTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddCustomer()
    {
        var repo = new FakeGenericRepository<Customer>();
        var manager = new CustomerManager(repo, _mapper);

        var dto = new CustomerCreateDto
        {
            FirstName = "Test",
            LastName = "Müşteri",
            Phone = "5550000000",
            Email = "test@example.com",
            Address = "Adres",
            TaxNo = "12345678901"
        };

        var created = await manager.CreateAsync(dto);
        Assert.NotNull(created);
        Assert.True(created.Id > 0);

        var all = await repo.GetAllAsync();
        Assert.Single(all);
        Assert.Equal("Test", all.First().FirstName);
    }
}
