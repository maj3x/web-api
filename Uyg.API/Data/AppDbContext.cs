using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uyg.API.Models;

namespace Uyg.API.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
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

        modelBuilder.Entity<News>()
            .HasOne(n => n.Category)
            .WithMany(c => c.News)
            .HasForeignKey(n => n.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<News>()
            .HasOne(n => n.Author)
            .WithMany()
            .HasForeignKey(n => n.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<News>()
            .HasMany(n => n.TagList)
            .WithMany(t => t.News)
            .UsingEntity(j => j.ToTable("NewsTags"));

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.News)
            .WithMany(n => n.Comments)
            .HasForeignKey(c => c.NewsId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Author)
            .WithMany()
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Media>()
            .HasOne(m => m.News)
            .WithMany(n => n.Media)
            .HasForeignKey(m => m.NewsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 