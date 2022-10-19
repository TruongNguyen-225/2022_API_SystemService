﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.Table
{
    public partial class HistoryExportData
    {
        [Key]
        public int HistoryID { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(250)]
        public string ServiceName { get; set; }
        [StringLength(250)]
        public string RetailName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        [StringLength(2000)]
        public string Query { get; set; }
        public int? Money { get; set; }
        public int? Postage { get; set; }
        public int? Total { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeAdd { get; set; }
    }
}