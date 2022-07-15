﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SystemServiceAPI.Entities.Table
{
    [Table("MonthlyTransaction")]
    public partial class MonthlyTransaction
    {
        [Key]
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int ServiceID { get; set; }
        public int RetailID { get; set; }
        public int? BankID { get; set; }
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        public double Money { get; set; }
        public double Postage { get; set; }
        public double Total { get; set; }
        public int? Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateTimeAdd { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeUpdate { get; set; }
    }
}