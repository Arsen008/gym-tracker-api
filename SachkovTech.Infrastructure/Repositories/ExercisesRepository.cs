using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Application.Exercises;
using SachkovTech.Domain;
using SachkovTech.Domain.Exercises;
using SachkovTech.Domain.Exercises.ValueObjects;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.Ids;

namespace SachkovTech.Infrastructure.Repositories;

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