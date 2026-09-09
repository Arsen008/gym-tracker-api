using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;  
using SachkovTech.Application.Exercises;
using SachkovTech.Domain;
using SachkovTech.Domain.Exercises;
using SachkovTech.Domain.Exercises.ValueObjects;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.Ids;

namespace SachkovTech.Application.Exercises.CreateExercise;

public class CreateExerciseHandler(
    IExercisesRepository exercisesRepository,
    IExerciseTypesRepository exerciseTypesRepository,
    ILogger<CreateExerciseHandler> logger) 
{
    public async Task<Result<Guid, Error>> Handle(
        CreateExerciseRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Попытка создания упражнения: {ExerciseName} ({MuscleGroup}) для типа {ExerciseTypeId}", 
            request.Name, request.MuscleGroup, request.ExerciseTypeId);
 
        var nameResult = ExerciseName.Create(request.Name);
        if (nameResult.IsFailure)
            return nameResult.Error;

        var muscleGroupResult = MuscleGroup.Create(request.MuscleGroup);
        if (muscleGroupResult.IsFailure)
            return muscleGroupResult.Error;

        var exerciseTypeId = ExerciseTypeId.Create(request.ExerciseTypeId);

    
        var exerciseType = await exerciseTypesRepository.GetByIdAsync(exerciseTypeId, cancellationToken);
        if (exerciseType is null)
        {
            logger.LogWarning("Вид упражнения с ID {ExerciseTypeId} не найден", request.ExerciseTypeId);
            return Errors.General.NotFound(request.ExerciseTypeId); 
        }

      
        var existingExercise = await exercisesRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existingExercise.IsSuccess)
        {
            logger.LogWarning("Упражнение {ExerciseName} уже существует в базе", request.Name);
            return Errors.General.AlreadyExists($"Exercise with name '{request.Name}'");
        }

     
        var exerciseId = ExerciseId.NewId();
        var exerciseResult = Exercise.Create(
            exerciseId, 
            exerciseTypeId, 
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