﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SystemServiceAPI.Entities.View
{
    public partial class vw_HistoryExport
    {
        public int HistoryID { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        public int? ServiceID { get; set; }
        [StringLength(50)]
        public string ServiceName { get; set; }
        public int? RetailID { get; set; }
        [StringLength(200)]
        public string RetailName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        [StringLength(2000)]
        public string Query { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeAdd { get; set; }
    }
}