﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.Table
{
    [Table("Cash")]
    public partial class Cash
    {
        [Key]
        public int CashID { get; set; }
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string Address { get; set; }
        public int ServiceID { get; set; }
        public double Money { get; set; }
        public double Postage { get; set; }
        public double Total { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateTimeAdd { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeUpdate { get; set; }
        public int RetailID { get; set; }
    }
}