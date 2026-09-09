using FluentValidation;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Exercises.DeleteExercise;

public class DeleteExerciseRequestValidator : AbstractValidator<DeleteExerciseRequest>
{
    public DeleteExerciseRequestValidator()
    {
        RuleFor(d => d.ExerciseId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("ExerciseId"));
    }
}