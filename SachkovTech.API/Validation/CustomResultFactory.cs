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

        // Авторитетный список сообщений, порождённых именно FluentValidation-правилами
        // для этого запроса. Всё, чего здесь нет, пришло от model binding
        // (System.Text.Json) или встроенной DataAnnotations-валидации — такой текст
        // может содержать имена типов, Path/LineNumber/BytePositionInLine и наружу не идёт.
        var fluentValidationMessages = validationResults?.Values
            .SelectMany(result => result.Errors)
            .Select(failure => failure.ErrorMessage)
            .ToHashSet(StringComparer.Ordinal) ?? [];

        List<ResponseError> responseErrors = [];

        foreach (var (invalidField, validationErrors) in validationProblemDetails.Errors)
        {
            foreach (var errorMessage in validationErrors)
            {
                if (Error.TryDeserialize(errorMessage, out var error))
                {
                    // Сериализованный доменный Error из FluentValidation-валидатора.
                    responseErrors.Add(new ResponseError(error.Code, error.Message, invalidField));
                    continue;
                }

                if (fluentValidationMessages.Contains(errorMessage))
                {
                    // Голое правило FluentValidation ("'Name' must not be empty") — не секретно.
                    responseErrors.Add(new ResponseError(InvalidValueCode, errorMessage, invalidField));
                    continue;
                }

                // Сообщение model binding / DataAnnotations — потенциальная утечка.
                // Клиенту — нейтральный текст, исходный — только в лог.
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
