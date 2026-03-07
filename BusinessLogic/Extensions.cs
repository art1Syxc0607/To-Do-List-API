using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using BusinessLogic.Services.Jwt;
using Microsoft.Extensions.DependencyInjection;

namespace BussinessLogic;

public static class Extensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IJwtService, JwtService>();
        serviceCollection.AddScoped<IAuthService, AuthService>();
        //serviceCollection.AddScoped<INoteService, NoteService>();

        return serviceCollection;
    }
}