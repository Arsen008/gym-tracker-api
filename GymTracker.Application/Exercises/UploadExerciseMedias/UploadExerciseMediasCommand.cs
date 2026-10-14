namespace GymTracker.Application.Exercises.UploadExerciseMedias;

public record UploadExerciseMediasCommand(Guid ExerciseId , IEnumerable<UploadFileDto> Files);