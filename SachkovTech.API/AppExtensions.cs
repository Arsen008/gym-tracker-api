using System.Collections.Immutable;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Infrastructure; 
 
using SachkovTech.API.Middlewares;
 
namespace SachkovTech.API.Extensions;

public static class AppExtensions
{
    public static async Task<WebApplication> ApplyMigration(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();

        return app;
    }

   
}