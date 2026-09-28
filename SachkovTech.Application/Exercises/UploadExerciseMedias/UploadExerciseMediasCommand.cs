namespace SachkovTech.Application.Exercises.UploadExerciseMedias;

public record UploadExerciseMediasCommand(Guid ExerciseId , IEnumerable<UploadFileDto> Files);