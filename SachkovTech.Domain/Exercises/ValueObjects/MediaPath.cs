using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;
namespace SachkovTech.Domain.Exercises.ValueObjects;

public record MediaPath
{
    public string Value { get;   }
    
    private MediaPath(string value)
        {
        Value = value;
        }
    public static Result<MediaPath, Error> Create(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Errors.General.ValueIsInvalid("Media Path");

        return new MediaPath(path);
    }
}