using FluentValidation;
using GymTracker.Application.Validation;
using GymTracker.Domain.Shared;
using GymTracker.Domain.Workouts.ValueObjects;

namespace GymTracker.Application.Workouts.CreateWorkout;

public class CreateWorkoutRequestValidator : AbstractValidator<CreateWorkoutRequest>
{
    public CreateWorkoutRequestValidator()
    {
        RuleFor(x => x.Exercises)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("ExerciseId"));

        RuleForEach(x => x.Exercises)
            .SetValidator(new CreateExerciseDtoValidator());

        RuleForEach(x => x.Tags)
            .MustBeValueObject(Tag.Create);
    }
}