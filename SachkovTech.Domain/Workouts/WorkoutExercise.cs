using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.Ids;
using SachkovTech.Domain.Workouts.ValueObjects;

namespace SachkovTech.Domain.Workouts;

public class WorkoutExercise : Entity<WorkoutExerciseId>
{
    private WorkoutExercise() { }

    internal WorkoutExercise(
        WorkoutExerciseId id, 
        WorkoutExerciseType exerciseType, 
        Sets sets, 
        Reps reps, 
        Weight weight) : base(id)
    {
        ExerciseType = exerciseType;
        Sets = sets;
        Reps = reps;
        Weight = weight;
    }

    public WorkoutExerciseType ExerciseType { get; private set; } = null!;
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
        WorkoutExerciseType exerciseType, 
        Sets sets, 
        Reps reps, 
        Weight weight)
    {
        if (exerciseType == null)
            return Errors.General.ValueIsRequired("ExerciseType");

        return new WorkoutExercise(id, exerciseType, sets, reps, weight);
    }
}