using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.Table
{
    [Table("Bank")]
    public partial class Bank
    {
        [Key]
        public int BankID { get; set; }
        [StringLength(50)]
        public string ShortName { get; set; }
        [Required]
        [StringLength(200)]
        public string FullName { get; set; }
    }
}