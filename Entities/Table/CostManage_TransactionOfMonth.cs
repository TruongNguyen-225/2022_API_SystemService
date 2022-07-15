using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.Table
{
    [Table("CostManage_TransactionOfMonth")]
    public partial class CostManage_TransactionOfMonth
    {
        public int TransactionID { get; set; }
        public int CostDetailID { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfMonth { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double CostValue { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public int Position { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateTimeCreate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeUpdate { get; set; }
    }
}