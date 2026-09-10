using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Application;
using SachkovTech.Infrastructure;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SachkovTech.API.Validation;
using SachkovTech.API.Extensions;
using SachkovTech.API.Middlewares;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console()
        .WriteTo.Debug()
        .WriteTo.Seq(context.Configuration["Seq"] 
            ?? throw new InvalidOperationException("Seq URL is not configured in appsettings.json"))
        .Enrich.WithThreadId()
        .Enrich.WithEnvironmentName()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentUserName()
        .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning);
});

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation(configuration =>
{
    configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    await app.ApplyMigration();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();