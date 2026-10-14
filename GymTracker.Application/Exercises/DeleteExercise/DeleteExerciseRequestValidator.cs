using FluentValidation;
using GymTracker.Application.Validation;
using GymTracker.Domain.Shared;

namespace GymTracker.Application.Exercises.DeleteExercise;

public class DeleteExerciseRequestValidator : AbstractValidator<DeleteExerciseRequest>
{
    public DeleteExerciseRequestValidator()
    {
        RuleFor(d => d.ExerciseId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("ExerciseId"));
    }
}