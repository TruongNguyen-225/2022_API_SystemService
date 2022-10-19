using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SystemServiceAPICore3.Dto
{
    public class CustomerDto
    {
        [Key]
        [DataMember]
        public int CustomerID { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Hotline { get; set; }

        [DataMember]
        public bool IsDelete { get; set; }

        [DataMember]
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeAdd { get; set; }

        [DataMember]
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeUpdate { get; set; }

        [DataMember]
        public int? MonthPrint { get; set; }

        [DataMember]
        public int? YearPrint { get; set; }

        [DataMember]
        public int? IsPrint { get; set; }

        [DataMember]
        public int? RetailID { get; set; }

        [DataMember]
        public int? ServiceID { get; set; }

        [DataMember]
        public int? BankID { get; set; }

        [DataMember]
        public int? VillageID { get; set; }
    }
}
