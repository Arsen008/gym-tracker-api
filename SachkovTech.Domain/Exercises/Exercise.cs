using System;
using CSharpFunctionalExtensions;
using SachkovTech.Domain.Exercises.ValueObjects;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.Ids;

namespace SachkovTech.Domain.Exercises;

public class Exercise : Entity<ExerciseId> , ISoftDeletable
{
    private bool _isDeleted;
    
    private Exercise() { }

    internal Exercise(
        ExerciseId id, 
        ExerciseTypeId exerciseTypeId, 
        ExerciseName name, 
        MuscleGroup muscleGroup) : base(id)
    {
        ExerciseTypeId = exerciseTypeId;
        Name = name;
        MuscleGroup = muscleGroup;
    }
        
    public ExerciseTypeId ExerciseTypeId { get; private set; } = null!;
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
        ExerciseTypeId exerciseTypeId, 
        ExerciseName name, 
        MuscleGroup muscleGroup)
    {
        if (id == null || id.Value == Guid.Empty)
            return Errors.General.ValueIsInvalid("Exercise Id");

        if (exerciseTypeId == null || exerciseTypeId.Value == Guid.Empty)
            return Errors.General.ValueIsInvalid("Exercise Type Id");

        return new Exercise(id, exerciseTypeId, name, muscleGroup); 
    }

    public void Update(ExerciseName name, MuscleGroup muscleGroup)
    {
        Name = name;
        MuscleGroup = muscleGroup;
    }
}