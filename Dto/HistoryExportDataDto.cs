using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SystemServiceAPICore3.Dto
{
    public class HistoryExportDataDto
    {
        [Key]
        [DataMember]
        public int HistoryID { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string ServiceName { get; set; }

        [DataMember]
        public string RetailName { get; set; }

        [DataMember]
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }

        [DataMember]
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }

        [DataMember]
        [StringLength(2000)]
        public string Query { get; set; }

        [DataMember]
        public int? Money { get; set; }

        [DataMember]
        public int? Postage { get; set; }

        [DataMember]
        public int? Total { get; set; }

        [DataMember]
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeAdd { get; set; }
    }
}
