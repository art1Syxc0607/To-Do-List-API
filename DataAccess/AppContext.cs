using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;

namespace DataAccess;

public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
{
    public DbSet<User> Users {  get; set; }
    public DbSet<TaskTo> Tasks {  get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}
