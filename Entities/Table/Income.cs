﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.Table
{
    [Table("Income")]
    public partial class Income
    {
        [Key]
        public int IncomeID { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Time { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool? IsEmploy { get; set; }
        public double? CostEmploy { get; set; }
        public double? Weight { get; set; }
        public double? UnitPrice { get; set; }
        public double? Concentration { get; set; }
        public double? IntoMoney { get; set; }
        public double? Remain { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateTimeAdd { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeUpdate { get; set; }
    }
}