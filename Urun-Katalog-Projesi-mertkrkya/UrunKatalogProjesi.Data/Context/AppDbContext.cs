using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrunKatalogProjesi.Data.Models;

namespace UrunKatalogProjesi.Data.Context
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        private static string _connectionstring;

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<AccountRefreshToken> AccountRefreshTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> BrandList { get; set; }
        public DbSet<Color> ColorList { get; set; }
        public static void SetContextConnectionString(string connectionstring)
        {
            _connectionstring = connectionstring;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseNpgsql(_connectionstring);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AccountRefreshToken>(entity =>
            {
                entity.HasKey(r => r.UserId);
                entity.ToTable("accountRefreshToken");
            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(r => r.Id).HasName("PK_CategoryId");
                entity.Property(r => r.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                entity.Property(r => r.CategoryName).IsRequired().HasMaxLength(128);
                entity.Property(r => r.CreatedBy).IsRequired();
                entity.Property(r => r.CreatedDate).IsRequired();
                entity.ToTable("category");
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(r => r.Id).HasName("PK_ProductId");
                entity.Property(r => r.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                entity.Property(r => r.ProductName).IsRequired().HasMaxLength(100);
                entity.Property(r => r.Description).IsRequired().HasMaxLength(500);
                entity.Property(r => r.CategoryId).IsRequired();
                entity.Property(r => r.ProductStatus).IsRequired();
                //################,## 18 numaradan oluşup son ikisi virgülle ayrılabilir.
                entity.Property(r => r.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(r => r.CreatedBy).IsRequired();
                entity.Property(r => r.CreatedDate).IsRequired();
                entity.HasOne(r => r.CategoryParentNavigation).WithMany(r => r.Products).HasForeignKey(r => r.CategoryId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(r => r.UserParentNavigation).WithMany(r => r.Products).HasForeignKey(r => r.OwnerId).OnDelete(DeleteBehavior.Cascade);
                entity.ToTable("product");
            });
            modelBuilder.Entity<Offer>(entity =>
            {
                entity.HasKey(r => r.Id).HasName("PK_OfferId");
                entity.Property(r => r.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                entity.Property(r => r.OfferPrice).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(r => r.OfferPercent).IsRequired().HasColumnType("decimal(4,2)");
                entity.Property(r => r.CreatedBy).IsRequired();
                entity.Property(r => r.CreatedDate).IsRequired();
                entity.HasOne(r => r.ProductParentNavigation).WithMany(r => r.Offers).HasForeignKey(r => r.ProductId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(r => r.UserParentNavigation).WithMany(r => r.Offers).HasForeignKey(r => r.OfferUserId).OnDelete(DeleteBehavior.Cascade);
                entity.ToTable("offer");
            });
        }
    }
}