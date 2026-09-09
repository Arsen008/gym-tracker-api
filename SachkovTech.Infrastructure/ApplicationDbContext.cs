using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SachkovTech.Domain;
using SachkovTech.Domain.Exercises; 
using SachkovTech.Domain.Workouts;
using SachkovTech.Infrastructure.Interceptors; 

namespace SachkovTech.Infrastructure;

public class ApplicationDbContext : DbContext
{
    private static readonly ILoggerFactory ConsoleLoggerFactory 
        = LoggerFactory.Create(builder => builder.AddConsole());

    private readonly SoftDeleteInterceptor _softDeleteInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        SoftDeleteInterceptor softDeleteInterceptor) : base(options)
    {
        _softDeleteInterceptor = softDeleteInterceptor;
    }

    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<ExerciseType> ExerciseTypes => Set<ExerciseType>();
    public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
    public DbSet<Exercise> Exercises => Set<Exercise>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        optionsBuilder.UseLoggerFactory(ConsoleLoggerFactory);
        
        optionsBuilder.AddInterceptors(_softDeleteInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}