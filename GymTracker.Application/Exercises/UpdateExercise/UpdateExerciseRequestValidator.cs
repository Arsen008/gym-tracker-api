using FluentValidation;
using GymTracker.Application.Validation;
using GymTracker.Domain.Exercises.ValueObjects;
using GymTracker.Domain.Shared;

namespace GymTracker.Application.Exercises.UpdateExercise;

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