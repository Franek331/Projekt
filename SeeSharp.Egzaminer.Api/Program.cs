using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeeSharp.Egzaminer.Domain.Entities;
using SeeSharp.Egzaminer.Infrastructure.Persistence;
using System.Security.Claims;
using SeeSharp.Egzaminer.Application.Interfaces;
using SeeSharp.Egzaminer.Infrastructure.Configuration;
using SeeSharp.Egzaminer.Infrastructure.Repositories;
using SeeSharp.Egzaminer.Api.Controllers;

namespace SeeSharp.Egzaminer.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"))
            .EnableSensitiveDataLogging().EnableDetailedErrors());

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        var jwtOptions = builder.Configuration.GetSection("JwtOptions");
        builder.Services.Configure<JwtOptions>(jwtOptions);

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Admin"));
        });

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddScoped<DbContext, AppDbContext>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        builder.Services.AddScoped(typeof(IKeyRepository<,>), typeof(KeyRepository<,>));

        // Interfejs tworzenia testów        
        builder.Services.AddScoped<ITestService, TestService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
            app.MapOpenApi();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Swagger"));
        }

        app.UseHttpsRedirection();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseAuthorization();
        app.MapControllers();
        app.MapFallbackToFile("index.html");

        using (var dbContext = app.Services.CreateScope().ServiceProvider.GetService<AppDbContext>())
        {
            dbContext?.Database.Migrate();

            TestDataSeeder.Seed(dbContext!);
        }

        app.Run();
    }
}
