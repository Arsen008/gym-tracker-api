using Microsoft.Extensions.Logging;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Application.Workouts;
using SachkovTech.Domain;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Workouts;
namespace SachkovTech.Application.Workouts.DeleteWorkout;

public class DeleteWorkoutHandler(
    IWorkoutsRepository workoutsRepository,
    ILogger<DeleteWorkoutHandler> logger)
{
    public async Task<UnitResult<Error>> Handle(
        DeleteWorkoutRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Попытка удаления воркаута с ID: {WorkoutId}", request.WorkoutId);

        var workoutId = WorkoutId.Create(request.WorkoutId);

        var workoutResult = await workoutsRepository.GetByIdAsync(workoutId, cancellationToken);
        if (workoutResult.IsFailure)
        {
            logger.LogWarning("Воркаут с ID {WorkoutId} не найден для удаления", request.WorkoutId);
            return Errors.General.NotFound(request.WorkoutId);
        }

        await workoutsRepository.DeleteAsync(workoutResult.Value, cancellationToken);

        logger.LogInformation("Воркаут с ID {WorkoutId} успешно удален", request.WorkoutId);

        return UnitResult.Success<Error>();
    }
}
