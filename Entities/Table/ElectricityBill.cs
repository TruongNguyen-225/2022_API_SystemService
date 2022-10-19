﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.Table
{
    [Table("ElectricityBill")]
    public partial class ElectricityBill
    {
        [Key]
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int ServiceID { get; set; }
        public double Money { get; set; }
        public double Postage { get; set; }
        public double Total { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateTimeAdd { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeUpdate { get; set; }
        public int RetailID { get; set; }
        public int? Stage { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}