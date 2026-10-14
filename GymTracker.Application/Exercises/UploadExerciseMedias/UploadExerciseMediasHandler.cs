using CSharpFunctionalExtensions;
using GymTracker.Application.Models;
using GymTracker.Application.Providers;
using GymTracker.Domain;
using GymTracker.Domain.Exercises.ValueObjects;
using GymTracker.Domain.Shared;
namespace GymTracker.Application.Exercises.UploadExerciseMedias;

public class UploadExerciseMediasHandler
{
    private readonly IFileProvider _fileProvider;
    private readonly IExercisesRepository _exercisesRepository;

    public UploadExerciseMediasHandler(
        IFileProvider fileProvider,
        IExercisesRepository exercisesRepository)
    {
        _fileProvider = fileProvider;
        _exercisesRepository = exercisesRepository;
    }

    public async Task<Result<IReadOnlyList<string>, Error>> Handle(
        UploadExerciseMediasCommand command,
        CancellationToken ct)
    {
        var exerciseId = ExerciseId.Create(command.ExerciseId);

        var exerciseResult = await _exercisesRepository.GetByIdAsync(exerciseId, ct);
        if (exerciseResult.IsFailure)
            return exerciseResult.Error;

        var filesData = command.Files.Select(f =>
            new FileData(f.Stream, "exercise-media", $"{Guid.NewGuid()}{Path.GetExtension(f.FileName)}")).ToList();

        var uploadResult = await _fileProvider.UploadFiles(filesData, ct);
        if (uploadResult.IsFailure)
            return uploadResult.Error;

        var exercise = exerciseResult.Value;

        foreach (var file in uploadResult.Value)
        {
            var mediaPathResult = MediaPath.Create(file);
            if (mediaPathResult.IsFailure)
                return mediaPathResult.Error;
            exercise.AddMedia(mediaPathResult.Value);
        }

        await _exercisesRepository.SaveAsync(exercise, ct);

        return uploadResult;
    }
}
    


