namespace GymTracker.Application.Exercises.CreateExercise;

public record CreateExerciseRequest(
    string Name,
    string MuscleGroup
);