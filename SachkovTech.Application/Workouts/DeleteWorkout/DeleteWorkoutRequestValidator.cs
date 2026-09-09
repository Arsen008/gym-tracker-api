using FluentValidation;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Workouts.DeleteWorkout;

public class DeleteWorkoutRequestValidator : AbstractValidator<DeleteWorkoutRequest>
{
    public DeleteWorkoutRequestValidator()
    {
        RuleFor(x => x.WorkoutId).NotEmpty().WithError(Errors.General.ValueIsRequired("WorkoutId"));
    }
}