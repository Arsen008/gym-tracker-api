using Microsoft.Extensions.DependencyInjection;
using GymTracker.Application.Workouts.CreateWorkout;
using GymTracker.Application.Exercises.CreateExercise;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using GymTracker.Application.Exercises.DeleteExercise;
using GymTracker.Application.Exercises.DeleteExerciseMedia;
using GymTracker.Application.Exercises.UpdateExercise;
using GymTracker.Application.Exercises.UploadExerciseMedia;
using GymTracker.Application.Exercises.UploadExerciseMedias;
using GymTracker.Application.Workouts.DeleteWorkout;
using GymTracker.Application.Workouts.UpdateWorkout;

namespace GymTracker.Application;


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
        services.AddScoped<UploadExerciseMediasHandler>();
        services.AddScoped<DeleteExerciseMediaHandler>();
        services.AddValidatorsFromAssemblyContaining<UpdateWorkoutRequestValidator>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
         
        return services;
    }
}