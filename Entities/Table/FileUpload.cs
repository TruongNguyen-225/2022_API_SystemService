using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPICore3.Entities.Table
{
    [Table("FileUpload")]
    public class FileUpload
    {
        [Key]
        public int ID { get; set; }

        public int ScreenID { get; set; }

        public string FileName { get; set; }

        public int FileSize { get; set; }

        public string FilePath { get; set; }

        public string FileType { get; set; }

        public DateTime? DateTimeAdd { get; set; }

        public DateTime? DateTimeUpdate { get; set; }
    }
}
