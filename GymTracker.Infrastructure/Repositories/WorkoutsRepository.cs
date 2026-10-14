using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using GymTracker.Application.Workouts;
using GymTracker.Domain;
using GymTracker.Domain.Shared;
using GymTracker.Domain.Workouts;

namespace GymTracker.Infrastructure.Repositories;

public class WorkoutsRepository(ApplicationDbContext dbContext) : IWorkoutsRepository
{
    public async Task<WorkoutId> AddAsync(Workout workout, CancellationToken cancellationToken = default)
    {
        await dbContext.Workouts.AddAsync(workout, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return workout.Id;
    }

    public async Task<Result<Workout, Error>> GetByIdAsync(WorkoutId id, CancellationToken cancellationToken = default)
    {
        var workout = await dbContext.Workouts
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

        if (workout is null)
            return Errors.General.NotFound(id.Value);

        return workout;
    }

    public async Task SaveAsync(Workout workout, CancellationToken cancellationToken = default)
    {
      
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Workout workout, CancellationToken cancellationToken = default)
    {
       
        workout.Delete();
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}