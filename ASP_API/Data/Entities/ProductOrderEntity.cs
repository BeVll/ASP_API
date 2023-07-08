using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_API.Data.Entities
{
    public class ProductOrderEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        public virtual OrderEntity Order { get; set; }
    }
}
