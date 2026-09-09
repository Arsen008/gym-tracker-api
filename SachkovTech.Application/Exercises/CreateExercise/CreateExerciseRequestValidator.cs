using FluentValidation;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Exercises.ValueObjects;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Exercises.CreateExercise;

public class CreateExerciseRequestValidator : AbstractValidator<CreateExerciseRequest>
{
    public CreateExerciseRequestValidator()
    {
        RuleFor(c => c.ExerciseTypeId).NotEmpty()
            .WithError(Errors.General
                .ValueIsRequired("ExerciseTypeId"));
        RuleFor(c => c.Name).MustBeValueObject(ExerciseName.Create);
        RuleFor(c => c.MuscleGroup).MustBeValueObject(MuscleGroup.Create);
    }
}