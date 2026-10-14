using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using GymTracker.Domain;
using GymTracker.Domain.Exercises;
using GymTracker.Domain.Exercises.ValueObjects;
using GymTracker.Domain.Shared;
using GymTracker.Domain.Shared.Ids;

namespace GymTracker.Application.Exercises.UpdateExercise;

public class UpdateExerciseHandler(
    IExercisesRepository exercisesRepository,
    ILogger<UpdateExerciseHandler> logger)
{
    public async Task<UnitResult<Error>> Handle(
        UpdateExerciseRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Попытка обновления упражнения с ID: {ExerciseId}", request.ExerciseId);

        var exerciseId = ExerciseId.Create(request.ExerciseId);

        var exerciseResult = await exercisesRepository.GetByIdAsync(exerciseId, cancellationToken);
        if (exerciseResult.IsFailure)
        {
            logger.LogWarning("Упражнение с ID {ExerciseId} не найдено", request.ExerciseId);
            return Errors.General.NotFound(request.ExerciseId);
        }

        var name = ExerciseName.Create(request.Dto.Name).Value;
        var muscleGroup = MuscleGroup.Create(request.Dto.MuscleGroup).Value;

        var exercise = exerciseResult.Value;
      
        exercise.Update(name, muscleGroup);
 
        await exercisesRepository.SaveAsync(exercise, cancellationToken);

        logger.LogInformation("Упражнение с ID {ExerciseId} успешно обновлено", request.ExerciseId);

        return UnitResult.Success<Error>();
    }
}
