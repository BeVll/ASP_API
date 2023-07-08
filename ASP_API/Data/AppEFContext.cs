using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ASP_API.Data.Entities;
using ASP_API.Data.Entities.Identity;

namespace ASP_API.Data
{
    public class AppEFContext : IdentityDbContext<UserEntity, RoleEntity, int,
        IdentityUserClaim<int>, UserRoleEntity, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public AppEFContext(DbContextOptions<AppEFContext> options)
            :base(options)
        {
            
        }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ImageProduct> ProductImages { get; set; }
        public DbSet<BasketEntity> Baskets { get; set; }
        public DbSet<DeliveryTypeEntity> DeliveryTypes { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ProductOrderEntity> OrderProducts { get; set; }
        public DbSet<DeliveryInfoEntity> DeliveriesInfo { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserRoleEntity>(ur =>
            {
                ur.HasKey(ur => new { ur.UserId, ur.RoleId });

                ur.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(r => r.RoleId)
                    .IsRequired();

                ur.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(u => u.UserId)
                    .IsRequired();
            });
            builder.Entity<BasketEntity>(ur =>
            {
                ur.HasKey(ur => new { ur.UserId, ur.ProductId });
            });
        }
    }
}
