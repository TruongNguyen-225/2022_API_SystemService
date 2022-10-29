using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemServiceAPI.Dto;
using SystemServiceAPI.Dto.CustomerID;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPICore3.Dto;

namespace SystemServiceAPICore3.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Account
            CreateMap<Account, AccountDto>();
            CreateMap<AccountDto, Account>();

            //Customer
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<AddCustomerDto, CustomerDto>();

            CreateMap<UpdateCustomerDto, Customer>().ReverseMap();
            CreateMap<UpdateCustomerDto, CustomerDto>().ReverseMap();
        }
    }
}
