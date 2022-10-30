using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.Table
{
    [Table("MonthlyTransaction")]
    public class MonthlyTransaction
    {
        [Key]
        public int ID { get; set; }

        public int CustomerID { get; set; }

        public int ServiceID { get; set; }

        public int RetailID { get; set; }

        public int? BankID { get; set; }

        [Required]
        [StringLength(50)]

        public string Code { get; set; }

        public int Money { get; set; }

        public int Postage { get; set; }

        public int Total { get; set; }

        public int? Status { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateTimeAdd { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DateTimeUpdate { get; set; }
    }
}