 
namespace GymTracker.Application.Exercises.UploadExerciseMedia;

public record UploadExerciseMediaCommand(Guid ExerciseId ,
    Stream Stream , string FileName);