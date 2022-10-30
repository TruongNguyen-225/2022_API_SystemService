using System;
namespace SystemServiceAPICore3.Dto.Other
{
    public class MonthlyTransactionResponse
    {
        public int STT { get; set; }

        public int ID { get; set; }

        public int ServiceID { get; set; }

        public string ServiceName { get; set;}

        public int CustomerID { get; set; }

        public string FullName { get; set; }

        public string Code { get; set; }

        public int RetailID { get; set; }

        public string RetailName { get; set; }

        public int? BankID { get; set; }

        public string BankName { get; set; }

        public int? Money { get; set; }

        public int? Postage { get; set; }

        public int? Total { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public DateTime? DateTimeAdd { get; set; }
    }
}

