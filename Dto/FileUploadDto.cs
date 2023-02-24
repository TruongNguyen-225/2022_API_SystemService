using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SystemServiceAPICore3.Dto
{
    public class FileUploadDto
    {
        [Key]
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int ScreenID { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public int FileSize { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string FileType { get; set; }

        [DataMember]
        public DateTime? DateTimeAdd { get; set; }

        [DataMember]
        public DateTime? DateTimeUpdate { get; set; }
    }
}
