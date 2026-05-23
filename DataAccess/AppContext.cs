using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;

namespace DataAccess;

public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
{
    public DbSet<User> Users {  get; set; }
    public DbSet<UserTask> Tasks {  get; set; }
    public DbSet<Category> Categories {  get; set; }
    public DbSet<Tag> Tags {  get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlite("Data Source=helloapp.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);
            
        });

        modelBuilder.Entity<UserTask>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                .HasMaxLength(200);

            entity.Property(t => t.Description)
                .HasMaxLength(1000);


            // Связь с User
            entity.HasOne(t => t.User)
                .WithMany(u => u.UserTasks)
                .HasForeignKey(t => t.UserId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // связь с Category
            entity.HasOne(t => t.Category)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            // связь с User
            entity.HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            

        });

        modelBuilder.Entity<Tag>(entity =>
        {
            // связь с User
            entity.HasOne(u => u.User)
            .WithMany(u => u.Tags)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);


            // Связь многие-ко-многим с UserTask
            entity.HasMany(t => t.UserTasks)
            .WithMany(u => u.Tags);
            
            

        });
    }
}
