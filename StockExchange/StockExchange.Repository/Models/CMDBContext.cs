using Microsoft.EntityFrameworkCore;
using StockExchange.Object.Tables;

namespace StockExchange.Repository.Models
{
    public partial class CMDBContext : DbContext
    {
        public CMDBContext()
        {
        }

        public CMDBContext(DbContextOptions<CMDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bwibbu> Bwibbu { get; set; }
        public virtual DbSet<Stocks> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bwibbu>(entity =>
            {
                entity.HasKey(e => new { e.Stockid, e.CreatedAt })
                    .HasName("PK_bwibbu_1");

                entity.ToTable("bwibbu");

                entity.Property(e => e.Stockid).HasColumnName("stockid");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("date");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(10);

                entity.Property(e => e.Pb).HasColumnName("pb");

                entity.Property(e => e.Pe).HasColumnName("pe");

                entity.Property(e => e.Reportyear)
                    .HasColumnName("reportyear")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Yield).HasColumnName("yield");

                entity.Property(e => e.Yieldyear).HasColumnName("yieldyear");
            });

            modelBuilder.Entity<Stocks>(entity =>
            {
                entity.ToTable("stocks");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });
        }
    }
}