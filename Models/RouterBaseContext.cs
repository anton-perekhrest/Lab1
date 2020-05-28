using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RouterLab
{
    public partial class RouterBaseContext : DbContext
    {
        public RouterBaseContext()
        {
        }

        public RouterBaseContext(DbContextOptions<RouterBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Diapason> Diapason { get; set; }
        public virtual DbSet<Price> Price { get; set; }
        public virtual DbSet<Router> Router { get; set; }
        public virtual DbSet<RouterStandart> RouterStandart { get; set; }
        public virtual DbSet<Speed> Speed { get; set; }
        public virtual DbSet<Standart> Standart { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=USER-PC\\SQLEXPRESS; Database=RouterBase; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diapason>(entity =>
            {
                entity.Property(e => e.Diapason1)
                    .IsRequired()
                    .HasColumnName("Diapason")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Price>(entity =>
            {
                entity.Property(e => e.Price1)
                    .IsRequired()
                    .HasColumnName("Price")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Router>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Diapason)
                    .WithMany(p => p.Router)
                    .HasForeignKey(d => d.DiapasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Router_Diapason");

                entity.HasOne(d => d.Price)
                    .WithMany(p => p.Router)
                    .HasForeignKey(d => d.PriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Router_Price");

                entity.HasOne(d => d.Speed)
                    .WithMany(p => p.Router)
                    .HasForeignKey(d => d.SpeedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Router_Speed");
            });

            modelBuilder.Entity<RouterStandart>(entity =>
            {
                entity.HasOne(d => d.Router)
                    .WithMany(p => p.RouterStandart)
                    .HasForeignKey(d => d.RouterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RouterStandart_Router");

                entity.HasOne(d => d.Standart)
                    .WithMany(p => p.RouterStandart)
                    .HasForeignKey(d => d.StandartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RouterStandart_Standart");
            });

            modelBuilder.Entity<Speed>(entity =>
            {
                entity.Property(e => e.Speed1)
                    .IsRequired()
                    .HasColumnName("Speed")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Standart>(entity =>
            {
                entity.Property(e => e.Standart1)
                    .IsRequired()
                    .HasColumnName("Standart")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
