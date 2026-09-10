using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;  
using SachkovTech.Application.Exercises;
using SachkovTech.Domain;
using SachkovTech.Domain.Exercises;
using SachkovTech.Domain.Exercises.ValueObjects;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Exercises.CreateExercise;

public class CreateExerciseHandler(
    IExercisesRepository exercisesRepository,
    ILogger<CreateExerciseHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        CreateExerciseRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Попытка создания упражнения: {ExerciseName} ({MuscleGroup})",
            request.Name, request.MuscleGroup);

        var nameResult = ExerciseName.Create(request.Name);
        if (nameResult.IsFailure)
            return nameResult.Error;

        var muscleGroupResult = MuscleGroup.Create(request.MuscleGroup);
        if (muscleGroupResult.IsFailure)
            return muscleGroupResult.Error;

        var existingExercise = await exercisesRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existingExercise.IsSuccess)
        {
            logger.LogWarning("Упражнение {ExerciseName} уже существует в базе", request.Name);
            return Errors.General.AlreadyExists($"Exercise with name '{request.Name}'");
        }

     
        var exerciseId = ExerciseId.NewId();
        var exerciseResult = Exercise.Create(
            exerciseId,
            nameResult.Value,
            muscleGroupResult.Value);

        if (exerciseResult.IsFailure)
        {
            logger.LogError("Ошибка доменной валидации при создании упражнения: {Error}", exerciseResult.Error.Message);
            return exerciseResult.Error;
        }

        await exercisesRepository.AddAsync(exerciseResult.Value, cancellationToken);

        logger.LogInformation("Упражнение {ExerciseName} успешно сохранено с ID: {ExerciseId}", request.Name, exerciseId.Value);

        return exerciseId.Value;
    }
}