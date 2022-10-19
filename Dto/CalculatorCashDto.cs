using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SystemServiceAPICore3.Dto
{
    public class CalculatorCashDto
    {
        [Key]
        [DataMember]
        public int CalculatorCashID { get; set; }

        [DataMember]
        public int? Budget { get; set; }

        [DataMember]
        public int? Borrow1 { get; set; }

        [DataMember]
        public int? Borrow2 { get; set; }
        
        [DataMember]
        public int? CashInHome1 { get; set; }
        
        [DataMember]
        public int? CashInHome2 { get; set; }
        
        [DataMember]
        public int? MoneyIntoViettelPay { get; set; }
        
        [DataMember]
        public int? CashFixedReatail2 { get; set; }
        
        [DataMember]
        public int? CashInRetail2 { get; set; }
        
        [DataMember]
        public int? MoneyIntoTPBank { get; set; }
        
        [DataMember]
        public int? MoneyIntoMBBank { get; set; }
        
        [DataMember]
        public int? Owe1 { get; set; }
        
        [DataMember]
        public int? Owe2 { get; set; }
        
        [DataMember]
        public int? Owe3 { get; set; }
        
        [DataMember]
        public int? Owe4 { get; set; }
        
        [DataMember]
        public int? SumCashHave { get; set; }
        
        [DataMember]
        public int? SumOwe { get; set; }
        
        [DataMember]
        public int? SumProfit { get; set; }
        
        [DataMember]
        [StringLength(500)]
        public string Notice { get; set; }
        
        [DataMember]
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeAdd { get; set; }
        
        [DataMember]
        [Column(TypeName = "datetime")]

        public DateTime? DatetTimeUpdate { get; set; }
    }
}
