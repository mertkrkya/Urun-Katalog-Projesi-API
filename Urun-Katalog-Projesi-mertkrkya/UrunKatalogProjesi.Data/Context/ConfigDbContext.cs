using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Models;

namespace UrunKatalogProjesi.Data.Context
{
    public class ConfigDbContext : DbContext
    {
        private static string _connectionstring;

        public ConfigDbContext()
        {
        }

        public ConfigDbContext(DbContextOptions<ConfigDbContext> options) : base(options)
        {
        }
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
            modelBuilder.HasDefaultSchema("Config");
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(r => r.Id).HasName("PK_BrandId");
                entity.Property(r => r.Name).IsRequired().HasMaxLength(128);
                entity.ToTable("brand");
            });
            modelBuilder.Entity<Color>(entity =>
            {
                entity.HasKey(r => r.Id).HasName("PK_ColorId");
                entity.Property(r => r.Name).IsRequired().HasMaxLength(128);
                entity.ToTable("color");
            });
        }
    }
}
