using CSharpFunctionalExtensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.API.Response;
using SachkovTech.Domain.Shared;

namespace SachkovTech.API.Extensions;

public static class ResponseExtensions
{
    public static ActionResult ToResponse(this ValidationResult result)
    {
        if (result.IsValid)
            throw new InvalidOperationException("Result cannot be successful");

        var validationErrors = result.Errors;

        var responseErrors = from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let error = Error.Deserialize(errorMessage)
            select new ResponseError(error.Code, error.Message, validationError.PropertyName);

        var envelope = Envelope.Error(responseErrors);

        return new ObjectResult(envelope)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }

    public static ActionResult ToResponse(this UnitResult<Error> result)
    {
        if (result.IsSuccess)
            return new OkResult();

        return result.Error.ToResponse();
    }

    public static ActionResult ToResponse<T>(this Result<T, Error> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(Envelope.Ok(result.Value));

        return result.Error.ToResponse();
    }

    public static ActionResult ToResponse(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        var responseError = new ResponseError(error.Code, error.Message, null);
        var envelope = Envelope.Error(responseError);

        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }

    public static ActionResult ToResponse(this IEnumerable<Error> errors)
    {
        var responseErrors = errors
            .Select(e => new ResponseError(e.Code, e.Message, null))
            .ToList();

        var envelope = Envelope.Error(responseErrors);

        return new ObjectResult(envelope)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}