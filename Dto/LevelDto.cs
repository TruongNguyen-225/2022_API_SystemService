using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SystemServiceAPICore3.Dto
{
    public class LevelDto
    {
        [Key]
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string TransactionLimit { get; set; }
    }
}
