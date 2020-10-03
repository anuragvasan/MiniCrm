using AutoMapper;
using MiniCrm.Application.Customer.Queries;
using MiniCrm.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCrm.Persistence.Customer.MapperProfiles
{
    /// <summary>
    /// Enables mapping an EF customer entity to a search query result.
    /// </summary>
    public class CustomerSearchResultProfile : Profile
    {
        public CustomerSearchResultProfile()
        {
            this.CreateMap<DataModel.Customer, SearchCustomers.CustomerSearchResult>()
                .ForPath(dest => dest.Address.Line1, cfg => cfg.MapFrom(src => src.AddressLine1))
                .ForPath(dest => dest.Address.Line2, cfg => cfg.MapFrom(src => src.AddressLine2))
                .ForPath(dest => dest.Address.City, cfg => cfg.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.State, cfg => cfg.MapFrom(src => src.State))
                .ForPath(dest => dest.Address.PostalCode, cfg => cfg.MapFrom(src => src.PostalCode))
                .ForPath(dest => dest.Phone.Number, cfg => cfg.MapFrom(src => src.PhoneNumber))
                .ForPath(dest => dest.Phone.Extension, cfg => cfg.MapFrom(src => src.PhoneExtension));
        }
    }
}
