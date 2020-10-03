using AutoMapper;
using MiniCrm.Application.Customer.Commands;
using MiniCrm.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCrm.Persistence.Customer.MapperProfiles
{
    /// <summary>
    /// Enables mapping a AddCustomer command to the Customer EF entity.
    /// </summary>
    public class AddCustomerProfile : Profile
    {
        public AddCustomerProfile()
        {
            this.CreateMap<AddCustomer, DataModel.Customer>()
                .ForMember(dest => dest.AddressLine1, cfg => cfg.MapFrom(src => src.Address.Line1))
                .ForMember(dest => dest.AddressLine2, cfg => cfg.MapFrom(src => src.Address.Line2))
                // todo: consider .IncludeMembers(src => src.Address)
                .ForMember(dest => dest.City, cfg => cfg.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.State, cfg => cfg.MapFrom(src => src.Address.State))
                .ForMember(dest => dest.PostalCode, cfg => cfg.MapFrom(src => src.Address.PostalCode))
                .ForMember(dest => dest.PhoneNumber, cfg => cfg.MapFrom(src => src.Phone.Number))
                .ForMember(dest => dest.PhoneExtension, cfg => cfg.MapFrom(src => src.Phone.Extension));
        }
    }
}
