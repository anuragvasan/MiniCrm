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
                .ForMember(dest => dest.Id, cfg => cfg.Ignore())
                .ForMember(dest => dest.AddressLine1, cfg => cfg.MapFrom(src => src.Address.Line1))
                .ForMember(dest => dest.AddressLine2, cfg => cfg.MapFrom(src => src.Address.Line2))
                .ForMember(dest => dest.City, cfg => cfg.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.State, cfg => cfg.MapFrom(src => src.Address.State))
                .ForMember(dest => dest.PostalCode, cfg => cfg.MapFrom(src => src.Address.PostalCode));
        }
    }
}
