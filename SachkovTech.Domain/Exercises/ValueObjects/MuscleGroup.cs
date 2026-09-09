using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;
namespace SachkovTech.Domain.Exercises.ValueObjects;

public record MuscleGroup
{
    public const int MaxLength = 50;

    private MuscleGroup(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<MuscleGroup, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MaxLength)
            return Errors.General.ValueIsInvalid("Muscle Group");

        return new MuscleGroup(value);
    }
}