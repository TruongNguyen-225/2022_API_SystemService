using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SystemServiceAPICore3.Dto
{
    public class CashDto
    {
        [Key]
        [DataMember]
        public int CashID { get; set; }
        
        [Required]
        [StringLength(50)]
        [DataMember]
        public string FullName { get; set; }
        
        [DataMember]
        [StringLength(50)]
        public string Address { get; set; }

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
    }
}
