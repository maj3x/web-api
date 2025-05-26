using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uyg.API.Models;

namespace Uyg.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<News> News { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure News entity
            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Summary).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.News)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Author)
                    .WithMany()
                    .HasForeignKey(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.TagList)
                    .WithMany(t => t.News)
                    .UsingEntity(j => j.ToTable("NewsTags"));
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Slug).HasMaxLength(200);

                entity.HasOne(e => e.ParentCategory)
                    .WithMany(c => c.SubCategories)
                    .HasForeignKey(e => e.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Comment entity
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();

                entity.HasOne(e => e.News)
                    .WithMany(n => n.Comments)
                    .HasForeignKey(e => e.NewsId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Author)
                    .WithMany()
                    .HasForeignKey(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ParentComment)
                    .WithMany(c => c.Replies)
                    .HasForeignKey(e => e.ParentCommentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Media entity
            modelBuilder.Entity<Media>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.FileUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.FileType).HasMaxLength(100);

                entity.HasOne(e => e.News)
                    .WithMany(n => n.Media)
                    .HasForeignKey(e => e.NewsId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Tag entity
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Slug).HasMaxLength(200);
            });
        }
    }
} 