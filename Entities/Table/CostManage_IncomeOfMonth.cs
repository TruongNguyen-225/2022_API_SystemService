using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.Table
{
    //[Keyless]
    [Table("CostManage_IncomeOfMonth")]
    public partial class CostManage_IncomeOfMonth
    {
        public int IncomeOfMonthID { get; set; }
        public int GroupIncomeID { get; set; }
        public double IncomeValue { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateTimeCreate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeUpdate { get; set; }
    }
}