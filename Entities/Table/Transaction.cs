﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SystemServiceAPI.Entities.Table
{
    [Table("Transaction")]
    public partial class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        public int TypeTransaction { get; set; }
        public double? Money { get; set; }
        [Required]
        [StringLength(200)]
        public string Note { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ForMonth { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeCreate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeUpdate { get; set; }
    }
}