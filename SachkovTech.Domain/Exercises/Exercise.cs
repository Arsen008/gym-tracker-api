using System;
using CSharpFunctionalExtensions;
using SachkovTech.Domain.Exercises.ValueObjects;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.Exercises;

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
    public MediaPath? MediaPath { get; private set; } 

    public void AddMedia(MediaPath mediaPath)
    {
        MediaPath = mediaPath;
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
        MediaPath = null;
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