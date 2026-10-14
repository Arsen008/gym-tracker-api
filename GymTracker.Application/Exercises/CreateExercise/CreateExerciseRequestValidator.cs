using FluentValidation;
using GymTracker.Application.Validation;
using GymTracker.Domain.Exercises.ValueObjects;
using GymTracker.Domain.Shared;

namespace GymTracker.Application.Exercises.CreateExercise;

public class CreateExerciseRequestValidator : AbstractValidator<CreateExerciseRequest>
{
    public CreateExerciseRequestValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(ExerciseName.Create);
        RuleFor(c => c.MuscleGroup).MustBeValueObject(MuscleGroup.Create);
    }
}