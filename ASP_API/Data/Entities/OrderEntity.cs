using ASP_API.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_API.Data.Entities
{
    [Table("tblOrders")]
    public class OrderEntity
    {
        [Key]
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime DataCreated { get; set; }
        public double Cost { get; set; }
        public double? DiscountCost { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public string Payment_Type { get; set; }
        public bool Payment_Status { get; set; }
        public string Recipient_Full_Name { get; set; }
        public string Recipient_Phone { get; set; }
        [ForeignKey("DeliveryInfo")]
        public int DeliveryInfoId { get; set; }
        public virtual DeliveryInfoEntity DeliveryInfo { get; set; }
        public virtual ICollection<ProductOrderEntity> Products { get; set; }
    }
}
