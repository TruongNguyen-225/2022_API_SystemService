﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.Table
{
    [Table("CostManage_CostDetail")]
    public partial class CostManage_CostDetail
    {
        public int CostDetailID { get; set; }
        public int GroupCostID { get; set; }
        [Required]
        [StringLength(50)]
        public string CostDetailName { get; set; }
        public double Estimate { get; set; }
        [Required]
        [StringLength(1000)]
        public string Descript { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateTimeCreate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateTimeUpdate { get; set; }
    }
}