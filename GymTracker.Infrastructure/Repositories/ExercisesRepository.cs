using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using GymTracker.Application.Exercises;
using GymTracker.Domain;
using GymTracker.Domain.Exercises;
using GymTracker.Domain.Exercises.ValueObjects;
using GymTracker.Domain.Shared;
using GymTracker.Domain.Shared.Ids;

namespace GymTracker.Infrastructure.Repositories;

public class ExercisesRepository(ApplicationDbContext dbContext) : IExercisesRepository
{
    public async Task<ExerciseId> AddAsync(Exercise exercise, CancellationToken cancellationToken = default)
    {
        await dbContext.Exercises.AddAsync(exercise, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return exercise.Id;
    }

    public async Task<Result<Exercise, Error>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var exerciseNameResult = ExerciseName.Create(name);
        if (exerciseNameResult.IsFailure)
            return Errors.General.ValueIsInvalid("Name");


        var exercise = await dbContext.Exercises
            .FirstOrDefaultAsync(e => e.Name == exerciseNameResult.Value, cancellationToken);

        if (exercise is null)
            return Errors.General.NotFound();

        return exercise;
    }

    public async Task<Result<Exercise, Error>> GetByIdAsync(
        ExerciseId id,
        CancellationToken cancellationToken = default)
    {
        var exercise = await dbContext.Exercises
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (exercise is null)
            return Errors.General.NotFound(id.Value);

        return exercise;
    }

    public async Task<ExerciseId> SaveAsync(Exercise exercise, CancellationToken cancellationToken = default)
    {
        dbContext.Exercises.Attach(exercise);
        await dbContext.SaveChangesAsync(cancellationToken);

        return exercise.Id;
    }

    public async Task<ExerciseId> DeleteAsync(Exercise exercise, CancellationToken cancellationToken = default)
    {
        dbContext.Exercises.Remove(exercise);
        await dbContext.SaveChangesAsync(cancellationToken);

        return exercise.Id;
    }
}