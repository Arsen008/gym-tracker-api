using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Application.Workouts.CreateWorkout;
using SachkovTech.Application.Exercises.CreateExercise;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Application.Exercises.DeleteExercise;
using SachkovTech.Application.Exercises.UpdateExercise;
using SachkovTech.Application.Exercises.UploadExerciseMedia;
using SachkovTech.Application.Workouts.DeleteWorkout;
using SachkovTech.Application.Workouts.UpdateWorkout;

namespace SachkovTech.Application;


public  static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateWorkoutHandler>();
        services.AddScoped<CreateExerciseHandler>();
        services.AddScoped<UpdateExerciseHandler>();
        services.AddScoped<DeleteExerciseHandler>();
        services.AddScoped<UpdateWorkoutHandler>();
        services.AddScoped<DeleteWorkoutHandler>();
        services.AddScoped<UploadExerciseMediaHandler>();
        services.AddValidatorsFromAssemblyContaining<UpdateWorkoutRequestValidator>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
         
        return services;
    }
}