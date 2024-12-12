using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PixelArtEditor.WinForms.Models
{
    public partial class PixelArtDBContext : DbContext
    {
        public PixelArtDBContext()
        {
        }

        public PixelArtDBContext(DbContextOptions<PixelArtDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ColorPalette> ColorPalettes { get; set; } = null!;
        public virtual DbSet<Frame> Frames { get; set; } = null!;
        public virtual DbSet<Pixel> Pixels { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:sql-pixelart-server.database.windows.net,1433;Initial Catalog=PixelArtDB;Persist Security Info=False;User ID=sqladmin;Password=Turorudi12345_;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ColorPalette>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ColorHex)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ColorName).HasMaxLength(50);

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ColorPalettes)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__ColorPale__Proje__693CA210");
            });

            modelBuilder.Entity<Frame>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DelayMs)
                    .HasColumnName("DelayMS")
                    .HasDefaultValueSql("((200))");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Frames)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__Frames__ProjectI__619B8048");
            });

            modelBuilder.Entity<Pixel>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ColorHex)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FrameId).HasColumnName("FrameID");

                entity.HasOne(d => d.Frame)
                    .WithMany(p => p.Pixels)
                    .HasForeignKey(d => d.FrameId)
                    .HasConstraintName("FK__Pixels__FrameID__656C112C");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
