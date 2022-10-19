using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.Table
{
    [Table("CostManage_Login")]
    public partial class CostManage_Login
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [StringLength(200)]
        public string Password { get; set; }
    }
}