namespace SachkovTech.Domain;

public record WorkoutId : IComparable<WorkoutId>
{
    public Guid Value { get; }
    public WorkoutId(Guid value) => Value = value;
    public static WorkoutId NewId() => new(Guid.NewGuid());
    public static WorkoutId Create(Guid value) => new(value);
    public static WorkoutId Empty => new(Guid.Empty);
    public int CompareTo(WorkoutId other) => Value.CompareTo(other.Value);
    
}