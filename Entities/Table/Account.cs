using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.Table
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int AccountID { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [StringLength(500)]
        public string Password { get; set; }
        public int Role { get; set; }
        public bool Active { get; set; }
        public string Email { get; set; }
        public string OTP { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? DateTimeAdd { get; set; }
    }
}