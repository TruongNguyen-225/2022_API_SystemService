using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SystemServiceAPICore3.Dto
{
    public class CostManage_CostDetailDto
    {
        [DataMember]
        public int CostDetailID { get; set; }

        [DataMember]
        public int GroupCostID { get; set; }

        [DataMember]
        public string CostDetailName { get; set; }

        [DataMember]
        public double Estimate { get; set; }

        [DataMember]
        public string Descript { get; set; }

        [DataMember]
        public DateTime DateTimeCreate { get; set; }

        [DataMember]
        public DateTime? DateTimeUpdate { get; set; }
    }
}
