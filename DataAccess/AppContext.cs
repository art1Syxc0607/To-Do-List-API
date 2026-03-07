using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;

namespace DataAccess;

public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
{
    public DbSet<User> Users {  get; set; }
    public DbSet<UserTask> Tasks {  get; set; }

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
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

        });
    }
}
