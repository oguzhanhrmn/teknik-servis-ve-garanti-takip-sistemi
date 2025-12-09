using AutoMapper;
using TSGTS.Core.DTOs;
using TSGTS.Core.Entities;

namespace TSGTS.Business.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<CustomerCreateDto, Customer>();

        CreateMap<Device, DeviceDto>().ReverseMap();
        CreateMap<DeviceCreateDto, Device>();

        CreateMap<ServiceTicket, ServiceTicketDto>().ReverseMap();
        CreateMap<ServiceTicketCreateDto, ServiceTicket>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<TicketStatus, TicketStatusDto>().ReverseMap();
    }
}
