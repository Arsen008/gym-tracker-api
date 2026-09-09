using SachkovTech.Domain.Shared.Ids;
namespace SachkovTech.Domain.Workouts.ValueObjects;

public record WorkoutExerciseType
{
    public ExerciseTypeId ExerciseTypeId { get; }
    public Guid ExerciseId { get; }

    public WorkoutExerciseType(ExerciseTypeId exerciseTypeId, Guid exerciseId)
    {
        ExerciseTypeId = exerciseTypeId;
        ExerciseId = exerciseId;
    }
}