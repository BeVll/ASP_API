using ASP_API.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_API.Data.Entities
{
    [Table("tblBaskets")]
    public class BasketEntity
    {
        public int Quintity { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual ProductEntity Product { get; set; }
    }
}
