using CSharpFunctionalExtensions;
using GymTracker.Domain.Shared;

namespace GymTracker.Domain.Workouts.ValueObjects;

public record Weight
{
    public double Value { get; }
    
    private Weight(double value) => Value = value;

    public static Result<Weight , Error> Create(double value)
    {
        if (value < 0)
            return  Errors.General.ValueIsInvalid("Weight");
            
        return new  Weight(value);
    }
}