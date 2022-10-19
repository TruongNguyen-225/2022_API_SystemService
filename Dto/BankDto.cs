using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SystemServiceAPICore3.Dto
{
    public class BankDto
    {
        [Key]
        [DataMember]
        public int BankID { get; set; }

        [DataMember]
        public string ShortName { get; set; }

        [DataMember]
        public string FullName { get; set; }
    }
}
