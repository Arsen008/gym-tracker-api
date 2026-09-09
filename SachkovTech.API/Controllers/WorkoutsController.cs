using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.API.Extensions;
using SachkovTech.Application.Exercises.UpdateExercise;
using SachkovTech.Application.Workouts.CreateWorkout;
using SachkovTech.Domain.Shared.Ids;
using SachkovTech.Application.Exercises.UpdateExercise;
using SachkovTech.Application.Workouts.DeleteWorkout;
using SachkovTech.Application.Workouts.UpdateWorkout;

namespace SachkovTech.API.Controllers;

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