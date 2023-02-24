using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPICore3.Entities.Table
{
    [Table("ConfigPrice")]
    public class ConfigPrice
    {
        [Key]
        public int ConfigID { get; set; }

        public int ServiceID { get; set; }

        public int LevelID { get; set; }

        public int Postage { get; set; }
    }
}
