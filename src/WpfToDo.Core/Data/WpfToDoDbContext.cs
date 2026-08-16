using Microsoft.EntityFrameworkCore;
using WpfToDo.Core.Models;

namespace WpfToDo.Core.Data;

public class WpfToDoDbContext : DbContext
{
    public WpfToDoDbContext(DbContextOptions<WpfToDoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<TodoList> TodoLists => Set<TodoList>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(50);
            entity.Property(x => x.ColorKey).IsRequired();
        });

        modelBuilder.Entity<TodoList>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(100);
            entity.Property(x => x.CreatedAt).IsRequired();
            entity.HasOne(x => x.Category)
                .WithMany(x => x.TodoLists)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Notes).HasMaxLength(2000);
            entity.Property(x => x.CreatedAt).IsRequired();
            entity.HasOne(x => x.TodoList)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.TodoListId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
