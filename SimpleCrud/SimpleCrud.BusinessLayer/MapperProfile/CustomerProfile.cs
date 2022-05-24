using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities = SimpleCrud.DataAccessLayer.Entities;
using SimpleCrud.Shared.Models.Responses;
using SimpleCrud.Shared.Models.Requests;

namespace SimpleCrud.BusinessLayer.MapperProfiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Entities.Customer, Customer>()
                .ForMember(destinationMember: dest => dest.FullName, memberOptions: opt => opt.MapFrom(mapExpression: src => $"{src.FirstName} {src.LastName}"));

            CreateMap<SaveCustomerRequest, Entities.Customer>();

            CreateMap<SaveCustomerRequest, Customer>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        }
    }
}
