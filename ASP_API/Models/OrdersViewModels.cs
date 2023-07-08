using ASP_API.Data.Entities.Identity;
using ASP_API.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static ASP_API.Models.UserViewModels;

namespace ASP_API.Models
{
    public class OrderCreateItemModel
    {
        public int UserId { get; set; }
        public string Payment_Type { get; set; }
        public string Recipient_Full_Name { get; set; }
        public string Recipient_Phone { get; set; }
        public List<ProductOrderCreateItemModel> Products { get; set; } = new List<ProductOrderCreateItemModel>();
        public int DeliveryTypeId { get; set; }
        public string Oblast { get; set; }
        public string Rayon { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string NumberOfHouse { get; set; }
    }
    public class OrderViewItemModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime DataCreated { get; set; }
        public double Cost { get; set; }
        public double? DiscountCost { get; set; }
        public int UserId { get; set; }
        public UserItemViewModel User { get; set; }
        public string Payment_Type { get; set; }
        public bool Payment_Status { get; set; }
        public string Recipient_Full_Name { get; set; }
        public string Recipient_Phone { get; set; }
        public int DeliveryInfoId { get; set; }
        public DeliveryInfoEntity DeliveryInfo { get; set; }
        public virtual ICollection<ProductOrderViewModel> Products { get; set; }
    }
    public class ProductOrderCreateItemModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class ProductOrderViewModel
    {
        public int Id { get; set; }
        public ProductItemViewModel Product { get; set; }
        public int Quantity { get; set; }
    }
}
