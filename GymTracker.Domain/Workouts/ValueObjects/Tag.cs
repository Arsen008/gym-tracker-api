using CSharpFunctionalExtensions;
using GymTracker.Domain.Shared;

namespace GymTracker.Domain.Workouts.ValueObjects;

public record Tag
{
    public const int MAX_LENGTH = 20;

    public string Value { get; }

    private Tag(string value)
    {
        Value = value;
    }

    public static Result<Tag, Error> Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Errors.General.ValueIsRequired("Tag");

        var trimmed = input.Trim();

        if (trimmed.Length > MAX_LENGTH)
            return Errors.General.InvalidLength("Tag");

        return new Tag(trimmed);
    }
}