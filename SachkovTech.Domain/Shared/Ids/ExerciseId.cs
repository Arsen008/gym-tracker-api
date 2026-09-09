namespace SachkovTech.Domain;

public record ExerciseId : IComparable<ExerciseId>
{
    public Guid Value { get; }
    private ExerciseId(Guid value) => Value = value;
    
    public static ExerciseId NewId() => new(Guid.NewGuid());
    public static ExerciseId Create(Guid value) => new(value);
    public static ExerciseId Empty => new(Guid.Empty);
    
    public int CompareTo(ExerciseId? other)
    {
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}
