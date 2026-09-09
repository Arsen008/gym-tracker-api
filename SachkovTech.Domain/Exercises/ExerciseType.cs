using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared.Ids;
using SachkovTech.Domain.Exercises;
using SachkovTech.Domain.Shared;
using CSharpFunctionalExtensions;
namespace SachkovTech.Domain.Exercises;

public class ExerciseType : Entity<ExerciseTypeId>
{
    public const int MAX_TITLE_LENGTH = 100;

    private readonly List<Exercise> _exercises = [];
    
    private ExerciseType() { }

    private ExerciseType(ExerciseTypeId id, string title) : base(id)
    {
        Title = title;
    }

    public string Title { get; private set; } = string.Empty;
    public IReadOnlyList<Exercise> Exercises => _exercises.AsReadOnly();

    public static Result<ExerciseType> Create(ExerciseTypeId id, string title)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > MAX_TITLE_LENGTH)
            return Result.Failure<ExerciseType>($"Название вида упражнения не может быть пустым или длиннее {MAX_TITLE_LENGTH} символов.");
        
        return Result.Success(new ExerciseType(id, title));
    }

    public UnitResult<Error> AddExercise(Exercise exercise)
    {
        if (_exercises.Any(e => e.Name.Value.Equals(exercise.Name.Value, StringComparison.OrdinalIgnoreCase)))
        {
            return Errors.General.AlreadyExists($"Упражнение с названием '{exercise.Name.Value}'");
        }

        _exercises.Add(exercise);
        return UnitResult.Success<Error>();
    }
}