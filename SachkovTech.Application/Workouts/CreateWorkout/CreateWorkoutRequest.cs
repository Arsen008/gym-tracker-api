namespace SachkovTech.Application.Workouts.CreateWorkout;

public record CreateWorkoutRequest(List<string>? Tags, List<CreateExerciseDto> Exercises);

public record CreateExerciseDto(
    Guid ExerciseTypeId,  
    Guid ExerciseId, 
    int Sets, 
    int Reps, 
    double Weight
);