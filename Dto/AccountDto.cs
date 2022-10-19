using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SystemServiceAPI.Dto
{
    public class AccountDto
    {
        [Key]
        [DataMember]
        public int AccountID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }
        
        [DataMember]
        public int Role { get; set; }
        
        [DataMember]
        public bool Active { get; set; }
        
        [DataMember]
        public string Email { get; set; }
        
        [DataMember]
        public string OTP { get; set; }
        
        [DataMember]
        public DateTime? LastLogin { get; set; }
        
        [DataMember]
        public DateTime? DateTimeAdd { get; set; }
    }
}