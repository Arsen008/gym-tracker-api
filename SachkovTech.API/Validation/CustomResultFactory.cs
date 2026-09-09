using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SachkovTech.API.Response;
using SachkovTech.Domain.Shared;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace SachkovTech.API.Validation;

public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
{
    public Task<IActionResult?> CreateActionResult(
        ActionExecutingContext context,
        ValidationProblemDetails? validationProblemDetails,
        IDictionary<IValidationContext, ValidationResult>? validationResults)
    {
        if (validationProblemDetails is null)
        {
            throw new InvalidOperationException("ValidationProblemDetails is null");
        }

        List<ResponseError> responseErrors = [];

        foreach (var (invalidField, validationErrors) in validationProblemDetails.Errors)
        {
            var errors = from errorMessage in validationErrors
                let error = Error.Deserialize(errorMessage)
                select new ResponseError(error.Code, error.Message, invalidField);

            responseErrors.AddRange(errors);
        }

        var envelope = Envelope.Error(responseErrors);

        IActionResult result = new ObjectResult(envelope)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };

        return Task.FromResult<IActionResult?>(result);
    }
}