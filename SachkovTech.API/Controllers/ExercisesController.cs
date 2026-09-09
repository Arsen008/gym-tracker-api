using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.API.Extensions;
using SachkovTech.Application.Exercises.CreateExercise;
using SachkovTech.Application.Exercises.DeleteExercise;
using SachkovTech.Application.Exercises.DeleteExerciseMedia;  
using SachkovTech.Application.Exercises.UpdateExercise;
using SachkovTech.Application.Exercises.UploadExerciseMedia;

namespace SachkovTech.API.Controllers;

public class ExercisesController : ApplicationController
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateExerciseRequest request,
        [FromServices] CreateExerciseHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return result.ToResponse();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateExerciseDto dto,
        [FromServices] UpdateExerciseHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new UpdateExerciseRequest(id, dto);
      
        var result = await handler.Handle(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
      
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteExerciseHandler handler,
        [FromServices] IValidator<DeleteExerciseRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new DeleteExerciseRequest(id);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToResponse();

        var result = await handler.Handle(request, cancellationToken);
        return result.ToResponse();
    }

    [HttpPost("{id:guid}/media")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadMedia(
        [FromRoute] Guid id,
        IFormFile file,
        [FromServices] UploadExerciseMediaHandler handler,
        CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл не был передан.");

        await using var stream = file.OpenReadStream();
        var command = new UploadExerciseMediaCommand(id, stream, file.FileName);

        var result = await handler.Handle(command, cancellationToken);
        
        return result.ToResponse();
    }

   
    [HttpDelete("{id:guid}/media")]
    public async Task<IActionResult> DeleteMedia(
        [FromRoute] Guid id,
        [FromServices] DeleteExerciseMediaHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteExerciseMediaCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        return result.ToResponse();
    }
}