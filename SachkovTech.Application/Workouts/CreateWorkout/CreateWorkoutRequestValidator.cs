using FluentValidation;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Workouts.ValueObjects;

namespace SachkovTech.Application.Workouts.CreateWorkout;

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