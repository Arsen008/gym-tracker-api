using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.Workouts.ValueObjects;  
public record Reps
{
    public int Value { get; }
    
    private Reps(int value) => Value = value;

    public static Result<Reps , Error> Create(int value)
    {
        if (value < 0)
            return Errors.General.ValueIsInvalid("Reps");
            
        return new Reps(value);
    }
}