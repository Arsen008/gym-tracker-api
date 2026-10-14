using CSharpFunctionalExtensions;
using GymTracker.Domain;
using GymTracker.Domain.Shared;
using GymTracker.Domain.Workouts;

namespace GymTracker.Application.Workouts;

public interface IWorkoutsRepository
{
    Task<WorkoutId> AddAsync(Workout workout, CancellationToken cancellationToken = default);
    Task<Result<Workout, Error>> GetByIdAsync(WorkoutId id, CancellationToken cancellationToken = default);
    Task SaveAsync(Workout workout, CancellationToken cancellationToken = default);
    Task DeleteAsync(Workout workout, CancellationToken cancellationToken = default);
}