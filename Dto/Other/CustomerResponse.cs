using System;
namespace SystemServiceAPICore3.Dto.Other
{
    public class CustomerResponse
    {
        public int STT { get; set; }

        public int ServiceID { get; set; }

        public string ServiceName { get; set; }

        public int? BankID { get; set; }

        public string BankName { get; set; }

        public int CustomerID { get; set; }

        public int RetailID { get; set; }

        public string RetailName { get; set; }

        public string FullName { get; set; }

        public string Code { get; set; }

        public int? VillageID { get; set; }

        public string Hotline { get; set; }

        public string Status { get; set; }

        public bool IsDelete { get; set; }

        public DateTime? DateTimeAdd { get; set; }

        public DateTime? DateTimeUpdate { get; set; }
    }
}

