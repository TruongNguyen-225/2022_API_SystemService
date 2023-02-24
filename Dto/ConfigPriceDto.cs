using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SystemServiceAPICore3.Dto
{
    public class ConfigPriceDto
    {
        [Key]
        [DataMember]
        public int ConfigID { get; set; }

        [DataMember]
        public int ServiceID { get; set; }

        [DataMember]
        public int LevelID { get; set; }

        [DataMember]
        public int Postage { get; set; }
    }
}
