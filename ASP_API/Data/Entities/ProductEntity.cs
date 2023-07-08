using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_API.Data.Entities
{
    [Table("tblProducts")]
    public class ProductEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required, StringLength(255)]
        public string Name { get; set; }
        public int Priority { get; set; }
        public double Price { get; set; }
        public double? DiscountPrice { get; set; }
        public bool IsDelete { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }


        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; }

        public virtual ICollection<ImageProduct> ProductImages { get; set; }
        public virtual ICollection<BasketEntity> Baskets { get; set; }

        public bool Status { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
