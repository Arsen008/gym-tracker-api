using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.Workouts.ValueObjects;

public record Sets
{
    public int Value { get; }
    
    private Sets(int value) => Value = value;

    public static Result<Sets , Error> Create(int value)
    {
        if (value <= 0)
            return Errors.General.ValueIsInvalid("Sets");
            
        return new Sets(value);
    }
}