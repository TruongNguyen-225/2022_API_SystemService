using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SystemServiceAPICore3.Dto
{
    public class MonthlyTransactionDto
    {
        [Key]
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int CustomerID { get; set; }

        [DataMember]
        public int ServiceID { get; set; }

        [DataMember]
        public int RetailID { get; set; }

        [DataMember]
        public int? BankID { get; set; }

        [DataMember]
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [DataMember]
        public int Money { get; set; }

        [DataMember]
        public int Postage { get; set; }

        [DataMember]
        public int Total { get; set; }

        [DataMember]
        public int? Status { get; set; }

        [DataMember]
        [Column(TypeName = "datetime")]
        public DateTime DateTimeAdd { get; set; }
        [Column(TypeName = "datetime")]

        [DataMember]
        public DateTime? DateTimeUpdate { get; set; }
    }
}
