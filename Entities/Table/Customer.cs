﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.Table
{
    [Table("Customer")]
    public partial class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        [StringLength(10)]
        public string Hotline { get; set; }
        public bool IsDelete { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeAdd { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeUpdate { get; set; }
        public int? MonthPrint { get; set; }
        public int? YearPrint { get; set; }
        public int? IsPrint { get; set; }
        public int? RetailID { get; set; }
        public int? ServiceID { get; set; }
        public int? BankID { get; set; }
        public int? VillageID { get; set; }
    }
}