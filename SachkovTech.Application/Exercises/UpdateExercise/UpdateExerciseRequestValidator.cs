using FluentValidation;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Exercises.ValueObjects;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Exercises.UpdateExercise;

public class UpdateExerciseRequestValidator : AbstractValidator<UpdateExerciseRequest>
{
    public UpdateExerciseRequestValidator()
    {
        RuleFor(r => r.ExerciseId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.Dto).SetValidator(new UpdateExerciseDtoValidator());
    }
}

public class UpdateExerciseDtoValidator : AbstractValidator<UpdateExerciseDto>
{
    public UpdateExerciseDtoValidator()
    {
        RuleFor(r => r.Name).MustBeValueObject(ExerciseName.Create);
        RuleFor(r => r.MuscleGroup).MustBeValueObject(MuscleGroup.Create);
    }
}