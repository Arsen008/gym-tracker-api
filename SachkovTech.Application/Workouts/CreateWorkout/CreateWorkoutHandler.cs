using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Application.Exercises;
using SachkovTech.Domain;
using SachkovTech.Domain.Exercises;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.Ids;
using SachkovTech.Domain.Workouts;
using SachkovTech.Domain.Workouts.ValueObjects;

namespace SachkovTech.Application.Workouts.CreateWorkout;

public class CreateWorkoutHandler(
    IWorkoutsRepository workoutsRepository,
    IExercisesRepository exercisesRepository,
    ILogger<CreateWorkoutHandler> logger)
{
    public async Task<Result<CreateWorkoutResponse, Error>> Handle(
        CreateWorkoutRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Создание новой тренировки. Количество упражнений: {Count}", request.Exercises.Count);

        var workoutId = WorkoutId.Create(Guid.NewGuid());
        var workoutResult = Workout.Create(workoutId, request.Tags);

        if (workoutResult.IsFailure)
            return workoutResult.Error;

        var workout = workoutResult.Value;

        foreach (var dto in request.Exercises)
        {
            var exerciseId = ExerciseId.Create(dto.ExerciseId);

            var existingExercise = await exercisesRepository.GetByIdAsync(exerciseId, cancellationToken);
            if (existingExercise.IsFailure)
            {
                logger.LogWarning("Упражнение с ID {ExerciseId} не найдено", dto.ExerciseId);
                return Errors.General.NotFound(dto.ExerciseId);
            }

            var exerciseResult = CreateWorkoutExercise(dto, exerciseId);

            if (exerciseResult.IsFailure)
                return exerciseResult.Error;

            var addResult = workout.AddExercise(exerciseResult.Value);
            if (addResult.IsFailure)
                return addResult.Error;
        }

        await workoutsRepository.AddAsync(workout, cancellationToken);

        logger.LogInformation("Тренировка {WorkoutId} успешно сохранена!", workoutId.Value);

        return new CreateWorkoutResponse(workoutId.Value);
    }

    private Result<WorkoutExercise, Error> CreateWorkoutExercise(
        CreateExerciseDto dto,
        ExerciseId exerciseId)
    {
        var setsResult = Sets.Create(dto.Sets);
        if (setsResult.IsFailure) return setsResult.Error;

        var repsResult = Reps.Create(dto.Reps);
        if (repsResult.IsFailure) return repsResult.Error;

        var weightResult = Weight.Create(dto.Weight);
        if (weightResult.IsFailure) return weightResult.Error;

        return WorkoutExercise.Create(
            WorkoutExerciseId.Create(Guid.NewGuid()),
            exerciseId,
            setsResult.Value,
            repsResult.Value,
            weightResult.Value);
    }
}