using CSharpFunctionalExtensions;

namespace SachkovTech.Domain;

public class ToDoItem
{
    public const int MAX_TITLE_LENGTH = 200;

  
    private ToDoItem()
    {
    }

    private ToDoItem(ToDoItemId id, string title, DateTime createdAt)
    {
        Id = id;
        Title = title;
        CreatedAt = createdAt;
    }

    public ToDoItemId Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public static Result<ToDoItem> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > MAX_TITLE_LENGTH)
        {
            return Result.Failure<ToDoItem>($"Title cannot be empty or exceed {MAX_TITLE_LENGTH} characters.");
        }

        var item = new ToDoItem(ToDoItemId.NewId(), title, DateTime.UtcNow);
        return Result.Success(item);
    }

    public Result UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle) || newTitle.Length > MAX_TITLE_LENGTH)
        {
            return Result.Failure($"Title cannot be empty or exceed {MAX_TITLE_LENGTH} characters.");
        }

        Title = newTitle;
        return Result.Success();
    }
}