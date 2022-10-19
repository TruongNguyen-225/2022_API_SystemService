using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SystemServiceAPICore3.Dto
{
    public class ElectricityBillDto
    {
        [Key]
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int CustomerID { get; set; }

        [DataMember]
        public int ServiceID { get; set; }

        [DataMember]
        public double Money { get; set; }

        [DataMember]
        public double Postage { get; set; }

        [DataMember]
        public double Total { get; set; }

        [DataMember]
        [Column(TypeName = "datetime")]
        public DateTime DateTimeAdd { get; set; }

        [DataMember]
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeUpdate { get; set; }

        [DataMember]
        public int RetailID { get; set; }

        [DataMember]
        public int? Stage { get; set; }

        [DataMember]
        public int? Month { get; set; }

        [DataMember]
        public int? Year { get; set; }
    }
}
