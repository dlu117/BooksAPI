using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace bookAPI.Model
{
    public partial class bookAPIContext : DbContext
    {
        public bookAPIContext()
        {
        }

        public bookAPIContext(DbContextOptions<bookAPIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<Word> Word { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:books-server.database.windows.net,1433;Initial Catalog=books;Persist Security Info=False;User ID=Daphne;Password=Password7;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.BookAuthor).IsUnicode(false);

                entity.Property(e => e.BookTitle).IsUnicode(false);

                entity.Property(e => e.ThumbnailUrl).IsUnicode(false);

                entity.Property(e => e.WebUrl).IsUnicode(false);
            });

            modelBuilder.Entity<Word>(entity =>
            {
                entity.Property(e => e.Word1).IsUnicode(false);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Word)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("BookId");
            });
        }
    }
}
