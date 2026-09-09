namespace SachkovTech.Domain.Shared.Ids;

public record ExerciseTypeId : IComparable<ExerciseTypeId> 
{
    public Guid Value { get; }
    private ExerciseTypeId(Guid value) => Value = value;
    
    public static ExerciseTypeId NewId() => new(Guid.NewGuid());
    public static ExerciseTypeId Create(Guid value) => new(value);
    public static ExerciseTypeId Empty => new(Guid.Empty);

 
    public int CompareTo(ExerciseTypeId? other)
    {
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}