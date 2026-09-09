using CSharpFunctionalExtensions;
using SachkovTech.Domain;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Workouts;

namespace SachkovTech.Application.Workouts;

public interface IWorkoutsRepository
{
    Task<WorkoutId> AddAsync(Workout workout, CancellationToken cancellationToken = default);
    Task<Result<Workout, Error>> GetByIdAsync(WorkoutId id, CancellationToken cancellationToken = default);
    Task SaveAsync(Workout workout, CancellationToken cancellationToken = default);
    Task DeleteAsync(Workout workout, CancellationToken cancellationToken = default);
}