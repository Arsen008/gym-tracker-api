namespace SachkovTech.Application.Exercises.UpdateExercise;
public record UpdateExerciseRequest( Guid ExerciseId, UpdateExerciseDto Dto );

public record UpdateExerciseDto( string Name,string MuscleGroup);