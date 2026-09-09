namespace SachkovTech.Application.Exercises.CreateExercise;

public record CreateExerciseRequest(
    Guid ExerciseTypeId, 
    string Name, 
    string MuscleGroup
);