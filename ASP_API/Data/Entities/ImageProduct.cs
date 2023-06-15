using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_API.Data.Entities
{
    [Table("tblProductImages")]
    public class ImageProduct
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(1000)]
        public string Path { get; set; }
        public bool IsMain { get; set; }
       
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }
    }
}
