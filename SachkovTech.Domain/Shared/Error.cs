namespace SachkovTech.Domain.Shared;

public record Error
{
    public const string SEPARATOR = "||";
    public string Code { get;  }
    public string Message { get;  }
    public ErrorType Type { get;  }

    private Error(string code, string message, ErrorType type)
    {
            Code = code;
            Message = message;
            Type = type;
    }
    public static Error NotFound(string code, string message) =>
    new  Error(code, message, ErrorType.NotFound);
    
    public static Error Failure(string code, string message) =>
        new  Error(code, message, ErrorType.Failure);
    
    public static Error Validation(string code, string message) =>
        new  Error(code, message, ErrorType.Validation);
    
    public static Error Conflict(string code, string message) =>
        new  Error(code, message, ErrorType.Conflict);

    public string Serialize()
    {
        return string.Join(SEPARATOR, Code, Message , Type);
    }

    public static Error Deserialize(string serialized)
    {
        if (TryDeserialize(serialized, out var error) == false)
            throw new ArgumentException("Invalid serialized format");

        return error;
    }

    public static bool TryDeserialize(string serialized, out Error error)
    {
        error = null!;

        var parts = serialized.Split(SEPARATOR);

        if (parts.Length < 3)
            return false;

        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
            return false;

        error = new Error(parts[0], parts[1], type);
        return true;
    }

}

public enum ErrorType 
{
    Validation,
    NotFound,
    Failure,
    Conflict
}