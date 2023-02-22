using System;

namespace SystemServiceAPI.Dto.CustomerDto
{
    public class CustomerRequestDto
    {
        public int? ServiceID { get; set; }
        public int? RetailID { get; set; }
    }

    public class AddCustomerDto
    {
        public int RetailID { get; set; }
        public int ServiceID { get; set; }
        public int? BankID { get; set; }
        public string FullName { get; set; } = String.Empty;
        public string Code { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public string Hotline { get; set; } = String.Empty;
        public bool IsDelete { get; set; } = false;
        public int? VillageID { get; set; }
        public DateTime DateTimeAdd { get; set; }
    }

    public class UpdateCustomerDto
    {
        public int CustomerID { get; set; }
        public int RetailID { get; set; }
        public int ServiceID { get; set; }
        public int? BankID { get; set; }
        public string FullName { get; set; } = String.Empty;
        public string Code { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public string Hotline { get; set; } = String.Empty;
        public int? VillageID { get; set; }
    }

    public class DeleteCustomerDto
    {
        public int ServiceID { get; set; }
        public string ListCustomerID { get; set; } = String.Empty;
    }
}
