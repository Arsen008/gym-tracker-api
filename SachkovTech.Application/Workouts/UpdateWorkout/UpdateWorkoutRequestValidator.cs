using FluentValidation;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Workouts.UpdateWorkout;

public class UpdateWorkoutRequestValidator : AbstractValidator<UpdateWorkoutRequest>
{
    public UpdateWorkoutRequestValidator()
    {
        RuleFor(w => w.WorkoutId).NotEmpty().WithError(Errors.General.ValueIsRequired("WorkoutId"));
        
        RuleFor(w => w.Dto.Tags).NotEmpty().WithError(Errors.General.ValueIsRequired("Tags"));
    }
}