using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;
namespace SachkovTech.Domain.Exercises.ValueObjects;

public record ExerciseName
{
    public const int MaxLength = 100;
    
    private   ExerciseName( string value )
    {
            Value = value;
    }
    public string Value { get; }

    public static Result<ExerciseName , Error> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > MaxLength)
            return Errors.General.ValueIsInvalid("Exercise Name");
        return new ExerciseName(name);
    }
}