using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using GymTracker.API.Extensions;
using GymTracker.Application.Exercises.UpdateExercise;
using GymTracker.Application.Workouts.CreateWorkout;
using GymTracker.Domain.Shared.Ids;
using GymTracker.Application.Exercises.UpdateExercise;
using GymTracker.Application.Workouts.DeleteWorkout;
using GymTracker.Application.Workouts.UpdateWorkout;

namespace GymTracker.API.Controllers;

public class WorkoutsController : ApplicationController
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateWorkoutRequest request,
        [FromServices] CreateWorkoutHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateWorkoutDto dto,
        [FromServices] UpdateWorkoutHandler handler,
        [FromServices] IValidator<UpdateWorkoutRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new UpdateWorkoutRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToResponse();

        var result = await handler.Handle(request, cancellationToken);
        return result.ToResponse();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteWorkoutHandler handler,
        [FromServices] IValidator<DeleteWorkoutRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new DeleteWorkoutRequest(id);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToResponse();

        var result = await handler.Handle(request, cancellationToken);
        return result.ToResponse();
    }
}