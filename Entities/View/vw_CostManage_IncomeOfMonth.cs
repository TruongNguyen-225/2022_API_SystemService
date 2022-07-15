﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SystemServiceAPI.Entities.View
{
    public partial class vw_CostManage_IncomeOfMonth
    {
        public int? IncomeOfMonthID { get; set; }
        public int? GroupIncomeID { get; set; }
        [Required]
        [StringLength(50)]
        public string GroupIncomeName { get; set; }
        public double Estimate { get; set; }
        public double? IncomeValue { get; set; }
        public double? DiffIncome { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeCreate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeUpdate { get; set; }
    }
}