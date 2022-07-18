﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SystemServiceAPI.Entities.Table
{
    [Table("CalculatorHistory")]
    public partial class CalculatorHistory
    {
        [Key]
        public int CalculatorCashID { get; set; }
        public int? Budget { get; set; }
        public int? Borrow1 { get; set; }
        public int? Borrow2 { get; set; }
        public int? CashInHome1 { get; set; }
        public int? CashInHome2 { get; set; }
        public int? MoneyIntoViettelPay { get; set; }
        public int? CashFixedReatail2 { get; set; }
        public int? CashInRetail2 { get; set; }
        public int? MoneyIntoTPBank { get; set; }
        public int? MoneyIntoMBBank { get; set; }
        public int? Owe1 { get; set; }
        public int? Owe2 { get; set; }
        public int? Owe3 { get; set; }
        public int? Owe4 { get; set; }
        [StringLength(500)]
        public string Owe1Note { get; set; }
        [StringLength(500)]
        public string Owe2Note { get; set; }
        [StringLength(500)]
        public string Owe3Note { get; set; }
        [StringLength(500)]
        public string Owe4Note { get; set; }
        public int? SumCashHave { get; set; }
        public int? SumOwe { get; set; }
        public int? SumProfit { get; set; }
        [StringLength(500)]
        public string Notice { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeAdd { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DatetTimeUpdate { get; set; }
    }
}