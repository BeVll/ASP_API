using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_API.Data.Entities
{
    [Table("tblDeliveriesInfo")]
    public class DeliveryInfoEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Delivery")]
        public int DeliveryTypeId { get; set; }
        public virtual DeliveryTypeEntity Delivery { get; set; }
        public string Oblast { get; set; }
        public string Rayon { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string NumberOfHouse { get; set; }


    }
}
