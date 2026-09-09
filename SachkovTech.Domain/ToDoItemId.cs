using CSharpFunctionalExtensions;

namespace SachkovTech.Domain;

public class ToDoItemId : ValueObject
{
    private ToDoItemId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ToDoItemId NewId() => new(Guid.NewGuid());

    public static ToDoItemId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}