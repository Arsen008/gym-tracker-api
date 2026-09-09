namespace SachkovTech.Domain.Shared.Ids;

public record WorkoutExerciseId : IComparable<WorkoutExerciseId>
{
    public Guid Value { get; }
    private WorkoutExerciseId(Guid value) => Value = value;
    
    public static WorkoutExerciseId NewId() => new(Guid.NewGuid());
    public static WorkoutExerciseId Create(Guid value) => new(value);
    public static WorkoutExerciseId Empty() => new(Guid.Empty);
    
    public int CompareTo(WorkoutExerciseId? other)
    {
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
    
}