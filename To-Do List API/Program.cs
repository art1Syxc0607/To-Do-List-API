using BussinessLogic;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// 1. Добавляем сервисы аутентификации с конкретной схемой JwtBearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // "Bearer"
    .AddJwtBearer(options => // Обязательно нужна конфигурация!
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
            ValidateIssuerSigningKey = true,
        };
    });

// 2. Добавляем авторизацию
builder.Services.AddAuthorization();
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddBusinessLogic(); 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Настройка Swagger для JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Пример: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
var app = builder.Build();




app.UseAuthentication(); // Проверяет токен, заполняет User
app.UseAuthorization();  // Проверяет права доступа ([Authorize])
app.UseStaticFiles();
app.MapControllers();
app.UseSwagger(); // htpps:localpost:...  /swagger/index.html
app.UseSwaggerUI();
app.Run();
