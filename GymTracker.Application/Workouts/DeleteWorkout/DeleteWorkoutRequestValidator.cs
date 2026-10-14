using FluentValidation;
using GymTracker.Application.Validation;
using GymTracker.Domain.Shared;

namespace GymTracker.Application.Workouts.DeleteWorkout;

public class DeleteWorkoutRequestValidator : AbstractValidator<DeleteWorkoutRequest>
{
    public DeleteWorkoutRequestValidator()
    {
        RuleFor(x => x.WorkoutId).NotEmpty().WithError(Errors.General.ValueIsRequired("WorkoutId"));
    }
}