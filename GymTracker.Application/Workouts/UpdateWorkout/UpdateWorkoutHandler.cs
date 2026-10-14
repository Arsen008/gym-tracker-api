using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using GymTracker.Domain;
using GymTracker.Domain.Shared;
using GymTracker.Domain.Workouts.ValueObjects;

namespace GymTracker.Application.Workouts.UpdateWorkout;

public class UpdateWorkoutHandler(IWorkoutsRepository workoutsRepository, ILogger<UpdateWorkoutHandler> logger)
{
    public async Task<UnitResult<Error>> Handle(
        UpdateWorkoutRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Попытка обновления воркаута с ID: {WorkoutId}", request.WorkoutId);
        
        var workoutId = WorkoutId.Create(request.WorkoutId);
        
        var workoutResult = await workoutsRepository.GetByIdAsync(workoutId, cancellationToken);
        if (workoutResult.IsFailure)
        {
            logger.LogWarning("Воркаут с ID {WorkoutId} не найден", request.WorkoutId);
            return Errors.General.NotFound(request.WorkoutId);
        }

        var workout = workoutResult.Value;
        
        workout.UpdateTags(request.Dto.Tags);

        await workoutsRepository.SaveAsync(workout, cancellationToken);

        logger.LogInformation("Воркаут с ID {WorkoutId} успешно обновлен", request.WorkoutId);

        return UnitResult.Success<Error>();
    }
}