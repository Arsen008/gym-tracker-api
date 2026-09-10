using FluentValidation;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Workouts.ValueObjects;
namespace SachkovTech.Application.Workouts.CreateWorkout;

public class CreateExerciseDtoValidator : AbstractValidator<CreateExerciseDto>
{
    public CreateExerciseDtoValidator()
    {
        RuleFor(x => x.ExerciseId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("ExerciseId"));

        RuleFor(x => x.Sets).MustBeValueObject(Sets.Create);
        RuleFor(x => x.Reps).MustBeValueObject(Reps.Create);
        RuleFor(x => x.Weight).MustBeValueObject(Weight.Create);
    }
}