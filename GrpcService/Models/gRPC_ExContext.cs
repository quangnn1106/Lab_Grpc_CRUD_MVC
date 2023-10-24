using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace GrpcService.Models
{
    public partial class gRPC_ExContext : DbContext
    {
        public gRPC_ExContext()
        {
        }

        public gRPC_ExContext(DbContextOptions<gRPC_ExContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=quanglaptop\\quangdev;uid=sa;pwd=123456;database=gRPC_Ex");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.Aid);

                entity.ToTable("Author");

                entity.Property(e => e.Aname).HasMaxLength(50);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Bid);

                entity.ToTable("Book");

                entity.Property(e => e.Bname).HasMaxLength(50);

                entity.HasOne(d => d.AidNavigation)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.Aid)
                    .HasConstraintName("FK_Book_Author");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
