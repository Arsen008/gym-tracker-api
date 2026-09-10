using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject, Error>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            Result<TValueObject, Error> result = factoryMethod(value);

            if (result.IsSuccess)
                return;

            context.AddFailure(new ValidationFailure(context.PropertyPath, result.Error.Message)
            {
                CustomState = result.Error,
                ErrorCode = result.Error.Code
            });
        });
    }

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule,
        Error error)
    {
        return rule.WithMessage(error.Message).WithState(_ => error);
    }
}