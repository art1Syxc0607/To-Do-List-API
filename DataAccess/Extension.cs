using DataAccess.Interfaces;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;          // Для UseSqlite и DbContextOptionsBuilder
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class Extensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration _configuration)
    {
        //services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddDbContext<AppContext>(options =>
        {
            //options.UseSqlite("Data Source=notes_persons.db");
            options.UseSqlite(_configuration["ConnectionStrings:DefaultConnection"]);

        });
        return services;
    }


}