using CSharpFunctionalExtensions;
using SachkovTech.Domain;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.Ids;
using SachkovTech.Domain.Workouts.ValueObjects;

namespace SachkovTech.Domain.Workouts;

public class WorkoutExercise : Entity<WorkoutExerciseId>
{
    private WorkoutExercise() { }

    internal WorkoutExercise(
        WorkoutExerciseId id,
        ExerciseId exerciseId,
        Sets sets,
        Reps reps,
        Weight weight) : base(id)
    {
        ExerciseId = exerciseId;
        Sets = sets;
        Reps = reps;
        Weight = weight;
    }

    public ExerciseId ExerciseId { get; private set; } = null!;
    public Sets Sets { get; private set; } = null!;
    public Reps Reps { get; private set; } = null!;
    public Weight Weight { get; private set; } = null!;

    internal Result UpdateMetrics(Sets sets, Reps reps, Weight weight)
    {
        Sets = sets;
        Reps = reps;
        Weight = weight;
        
        return Result.Success();
    }

    public static Result<WorkoutExercise, Error> Create(
        WorkoutExerciseId id,
        ExerciseId exerciseId,
        Sets sets,
        Reps reps,
        Weight weight)
    {
        if (exerciseId == null || exerciseId.Value == Guid.Empty)
            return Errors.General.ValueIsRequired("ExerciseId");

        return new WorkoutExercise(id, exerciseId, sets, reps, weight);
    }
}