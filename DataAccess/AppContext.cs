using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;

namespace DataAccess;

public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
{
    DbSet<User> users {  get; set; }  
    DbSet<TaskTo> tasks {  get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}
