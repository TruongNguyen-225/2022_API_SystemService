﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.Table
{
    //[Keyless]
    [Table("CostManage_GroupIncome")]
    public partial class CostManage_GroupIncome
    {
        public int GroupIncomeID { get; set; }
        [Required]
        [StringLength(50)]
        public string GroupIncomeName { get; set; }
        public double Estimate { get; set; }
        [StringLength(500)]
        public string Descript { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateTimeCreate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateTimeUpdate { get; set; }
    }
}