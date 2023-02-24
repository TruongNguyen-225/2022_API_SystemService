using AutoMapper;
using SystemServiceAPI.Dto;
using SystemServiceAPI.Dto.BillDto;
using SystemServiceAPI.Dto.CustomerDto;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPICore3.Dto;
using SystemServiceAPICore3.Dto.Other;
using SystemServiceAPICore3.Entities.Table;

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

            //MonthlyTransaction
            CreateMap<MonthlyTransaction, MonthlyTransactionDto>();
            CreateMap<MonthlyTransactionDto, MonthlyTransaction>();

            CreateMap<BillInsertDto, MonthlyTransaction>().ReverseMap();
            CreateMap<BillInsertDto, MonthlyTransactionTemp>().ReverseMap();

            CreateMap<BillUpdateDto, MonthlyTransaction>().ReverseMap();
            CreateMap<BillUpdateDto, MonthlyTransactionTemp>().ReverseMap();
            CreateMap<MonthlyTransactionResponse, MonthlyTransactionTemp>().ForMember(x => x.ID, opt => opt.Ignore()).ReverseMap();

            //Level
            CreateMap<Level, LevelDto>();
            CreateMap<LevelDto, Level>();

            //FileUpload
            CreateMap<FileUpload, FileUploadDto>();
            CreateMap<FileUploadDto, FileUpload>();

            //ConfigPrice
            CreateMap<ConfigPrice, ConfigPriceDto>();
            CreateMap<ConfigPriceDto, ConfigPrice>();

            //Screen
            CreateMap<Screen, ScreenDto>();
            CreateMap<ScreenDto, Screen>();
        }
    }
}
