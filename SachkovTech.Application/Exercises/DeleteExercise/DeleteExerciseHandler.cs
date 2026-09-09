using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Domain;
using SachkovTech.Domain.Exercises.ValueObjects;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Exercises.DeleteExercise;

public class DeleteExerciseHandler(
    IExercisesRepository exercisesRepository,
    ILogger<DeleteExerciseHandler> logger)
{
    public async Task<UnitResult<Error>> Handle(
        DeleteExerciseRequest request,
        CancellationToken cancellationToken = default)
    {
        
        logger.LogInformation("Попытка удаления упражнения с ID: {ExerciseId}", request.ExerciseId);

        var exerciseId = ExerciseId.Create(request.ExerciseId);

        var exerciseResult = await exercisesRepository.GetByIdAsync(exerciseId, cancellationToken);
        if (exerciseResult.IsFailure)
        {
            logger.LogWarning("Упражнение с ID {ExerciseId} не найдено для удаления", request.ExerciseId);
            return Errors.General.NotFound(request.ExerciseId);
        }

        await exercisesRepository.DeleteAsync(exerciseResult.Value, cancellationToken);

        logger.LogInformation("Упражнение с ID {ExerciseId} успешно удалено", request.ExerciseId);

        return UnitResult.Success<Error>();
    }
}