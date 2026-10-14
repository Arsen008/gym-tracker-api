using FluentValidation;
using GymTracker.Application.Validation;
using GymTracker.Domain.Shared;

namespace GymTracker.Application.Workouts.UpdateWorkout;

public class UpdateWorkoutRequestValidator : AbstractValidator<UpdateWorkoutRequest>
{
    public UpdateWorkoutRequestValidator()
    {
        RuleFor(w => w.WorkoutId).NotEmpty().WithError(Errors.General.ValueIsRequired("WorkoutId"));
        
        RuleFor(w => w.Dto.Tags).NotEmpty().WithError(Errors.General.ValueIsRequired("Tags"));
    }
}