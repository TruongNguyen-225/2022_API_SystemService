using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemServiceAPI.Entities.View
{
    public partial class vw_AllService
    {
        public int ID { get; set; }
        [StringLength(50)]
        public string BankName { get; set; }
        [StringLength(50)]
        public string FullName { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        public double Money { get; set; }
        public double Postage { get; set; }
        public double Total { get; set; }
        public int ServiceID { get; set; }
        [StringLength(50)]
        public string ServiceName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateTimeAdd { get; set; }
        public int RetailID { get; set; }
        [StringLength(200)]
        public string RetailName { get; set; }
        [StringLength(200)]
        public string DeputizeName { get; set; }
        [StringLength(10)]
        public string Hotline { get; set; }
        [StringLength(200)]
        public string AddressRetail { get; set; }
    }
}