using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.Table
{
    [Table("Village")]
    public partial class Village
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        public string VillageName { get; set; }
    }
}