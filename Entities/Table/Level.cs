using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPICore3.Entities.Table
{
    [Table("Level")]
    public class Level
    {
        [Key]
        public int ID { get; set; }

        public string TransactionLimit { get; set; }
    }
}
