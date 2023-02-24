using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPICore3.Entities.Table
{
    [Table("Screen")]
    public class Screen
    {
        [Key]
        public int ID { get; set; }

        public string ScreenName { get; set; }

        public string Describe { get; set; }
    }
}
