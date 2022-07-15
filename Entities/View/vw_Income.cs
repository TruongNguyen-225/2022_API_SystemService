﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SystemServiceAPI.Entities.View
{
    public partial class vw_Income
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int? DaysInMonth { get; set; }
        [StringLength(30)]
        public string Time { get; set; }
        public double? IntoMoney { get; set; }
        [StringLength(8000)]
        public string BillCode { get; set; }
        [Required]
        [StringLength(19)]
        public string RetailName { get; set; }
        [Required]
        [StringLength(24)]
        public string Address { get; set; }
        [Required]
        [StringLength(12)]
        public string Hotline { get; set; }
        [Required]
        [StringLength(17)]
        public string AccountName { get; set; }
        [StringLength(61)]
        public string DateTimeCreate { get; set; }
    }
}