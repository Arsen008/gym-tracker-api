using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SachkovTech.API.Response;
using SachkovTech.Domain.Shared;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace SachkovTech.API.Validation;

public class CustomResultFactory(ILogger<CustomResultFactory> logger)
    : IFluentValidationAutoValidationResultFactory
{
    private const string InvalidValueCode = "value.is.invalid";
    private const string NeutralInvalidValueMessage = "Invalid value format";

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

        // (1) Ошибки FluentValidation — типизированные ValidationFailure.
        // CustomState несёт доменный Error (см. CustomValidators), если правило его задало.
        var fluentValidationFailures = (validationResults?.Values ?? Enumerable.Empty<ValidationResult>())
            .SelectMany(result => result.Errors)
            .ToList();

        foreach (var failure in fluentValidationFailures)
        {
            responseErrors.Add(failure.CustomState is Error error
                ? new ResponseError(error.Code, error.Message, failure.PropertyName)
                : new ResponseError(InvalidValueCode, failure.ErrorMessage, failure.PropertyName));
        }

        // (2) Всё остальное в ModelState — это model binding (System.Text.Json) или
        // встроенная DataAnnotations-валидация. Такой текст может содержать имена типов,
        // Path/LineNumber/BytePositionInLine — наружу не идёт, только в лог.
        var fluentValidationMessages = fluentValidationFailures
            .Select(failure => failure.ErrorMessage)
            .ToHashSet(StringComparer.Ordinal);

        foreach (var (invalidField, validationErrors) in validationProblemDetails.Errors)
        {
            foreach (var errorMessage in validationErrors)
            {
                if (fluentValidationMessages.Contains(errorMessage))
                {
                    // Копия FluentValidation-фейла, уже добавлена в шаге (1).
                    continue;
                }

                logger.LogWarning(
                    "Ошибка привязки модели скрыта от клиента. Поле: {InvalidField}. Исходное сообщение: {RawMessage}",
                    invalidField,
                    errorMessage);

                responseErrors.Add(new ResponseError(InvalidValueCode, NeutralInvalidValueMessage, invalidField));
            }
        }

        var envelope = Envelope.Error(responseErrors);

        IActionResult result = new ObjectResult(envelope)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };

        return Task.FromResult<IActionResult?>(result);
    }
}
