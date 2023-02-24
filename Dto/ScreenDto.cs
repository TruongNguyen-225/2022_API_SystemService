using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SystemServiceAPICore3.Dto
{
    public class ScreenDto
    {
        [Key]
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string ScreenName { get; set; }

        [DataMember]
        public string Describe { get; set; }
    }
}
