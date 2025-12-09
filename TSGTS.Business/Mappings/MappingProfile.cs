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

        CreateMap<SparePart, SparePartDto>().ReverseMap();
        CreateMap<SparePartCreateDto, SparePart>();

        CreateMap<Invoice, InvoiceDto>().ReverseMap();
        CreateMap<InvoiceCreateDto, Invoice>();

        CreateMap<Payment, PaymentDto>().ReverseMap();
        CreateMap<PaymentCreateDto, Payment>();

        CreateMap<PaymentType, PaymentTypeDto>().ReverseMap();
    }
}
