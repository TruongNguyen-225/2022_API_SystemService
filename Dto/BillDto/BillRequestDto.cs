﻿using System;

namespace SystemServiceAPI.Dto.BillDto
{
    public class BillRequestDto
    {
        public int CustomerID { get; set; }
        public int ServiceID { get; set; }
        public int RetailID { get; set; }
        public int BankID { get; set; } = 80;
        public string Code { get; set; } = String.Empty;
        public int Money { get; set; }
        public int Postage { get; set; }
        public int Total { get; set; }
        public int Status { get; set; } = 1;
        public DateTime DateTimeAdd { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
    }

    public class BillUpdatetDto
    {
        public int ID { get; set; }
        public int RetailID { get; set; }
        public int BankID { get; set; } = 80;
        public int Money { get; set; }
        public int Postage { get; set; }
        public int Total { get; set; }
    }

    public class BillSearchDto
    {
        public string SearchText { get; set; } = String.Empty;
        public int ServiceID { get; set; }
    }

    public class BillFilterDto
    {
        public int ServiceID { get; set; }
        public int Month { get; set; }
    }

    public class BillDeleteDto
    {
        public int ServiceID { get; set; }
        public string ListBillID { get; set; } = String.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class BillPrintAllDto
    {
        public int ServiceID { get; set; }
        public string ListBillID { get; set; } = String.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
