using System;
using CSharpFunctionalExtensions;
using GymTracker.Domain.Exercises.ValueObjects;
using GymTracker.Domain.Shared;

namespace GymTracker.Domain.Exercises;

public class Exercise : Entity<ExerciseId> , ISoftDeletable
{
    private bool _isDeleted;
    
    private Exercise() { }

    internal Exercise(
        ExerciseId id,
        ExerciseName name,
        MuscleGroup muscleGroup) : base(id)
    {
        Name = name;
        MuscleGroup = muscleGroup;
    }

    public ExerciseName Name { get; private set; } = null!;
    public MuscleGroup MuscleGroup { get; private set; } = null!;

    private readonly List<MediaPath> _mediaPaths = [];
    
    public IReadOnlyList<MediaPath> MediaPaths => _mediaPaths;
    
 

    public void AddMedia(MediaPath mediaPath)
    {
            _mediaPaths.Add(mediaPath);
    }
    public void Delete()
    {
        if (_isDeleted ==  false)
            _isDeleted =  true;
    }

    public void Restore()
    {
        if (_isDeleted)
            _isDeleted = false;
    }

    public void ClearMedia()
    {
        _mediaPaths.Clear();
    }
    public static Result<Exercise, Error> Create(
        ExerciseId id,
        ExerciseName name,
        MuscleGroup muscleGroup)
    {
        if (id == null || id.Value == Guid.Empty)
            return Errors.General.ValueIsInvalid("Exercise Id");

        return new Exercise(id, name, muscleGroup);
    }

    public void Update(ExerciseName name, MuscleGroup muscleGroup)
    {
        Name = name;
        MuscleGroup = muscleGroup;
    }
}