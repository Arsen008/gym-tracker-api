using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Workouts;
using SachkovTech.Domain.Workouts.ValueObjects;

namespace SachkovTech.Domain;

public class Workout : Entity<WorkoutId>, ISoftDeletable
{
    private readonly List<WorkoutExercise> _exercises = [];

    private bool _isDeleted;
    
    private Workout()
    {
    }

    private Workout(WorkoutId id, TagsList tags) : base(id)
    {
        Tags = tags;
    }

    public TagsList Tags { get; private set; } = new(Array.Empty<string>());

    public IReadOnlyList<WorkoutExercise> Exercises => _exercises.AsReadOnly();

    public static Result<Workout, Error> Create(WorkoutId id, IEnumerable<string>? tags = null)
    {
        var tagsList = new TagsList(tags ?? Array.Empty<string>());

        return new Workout(id, tagsList);
    }

    public void UpdateTags(IEnumerable<string> tags)
    {
        Tags = new TagsList(tags);
    }

    public void Delete()
    {
        if (_isDeleted == false)
            _isDeleted = true;
            
    }

    public void Restore()
    {
        if (_isDeleted)
            _isDeleted = false;
    }
    public UnitResult<Error> AddExercise(WorkoutExercise exercise)
    {
        if (_exercises.Any(e => e.Id == exercise.Id))
            return Errors.General.AlreadyExists("Exercise");

        _exercises.Add(exercise);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateExercise(WorkoutExercise exercise)
    {
        var index = _exercises.FindIndex(e => e.Id == exercise.Id);
        
        if (index == -1)
            return Errors.General.NotFound(exercise.Id.Value);

        _exercises[index] = exercise;

        return UnitResult.Success<Error>();
    }
}