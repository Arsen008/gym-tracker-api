using CSharpFunctionalExtensions;
using SachkovTech.Application.Models;
using SachkovTech.Application.Providers;
using SachkovTech.Domain;
using SachkovTech.Domain.Exercises.ValueObjects;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Exercises.UploadExerciseMedia;

public class UploadExerciseMediaHandler
{
    private readonly IFileProvider _fileProvider;
    private readonly IExercisesRepository _exercisesRepository;

    public UploadExerciseMediaHandler(
        IFileProvider fileProvider,
        IExercisesRepository exercisesRepository)
    {
        _fileProvider = fileProvider;
        _exercisesRepository = exercisesRepository;
    }

    public async Task<Result<string, Error>> Handle(
        UploadExerciseMediaCommand command,
        CancellationToken ct)
    {
         
        var exerciseId = ExerciseId.Create(command.ExerciseId);

     
        var exerciseResult = await _exercisesRepository.GetByIdAsync(exerciseId, ct);
        if (exerciseResult.IsFailure)
            return exerciseResult.Error;

         
        var fileData = new FileData(command.Stream, "exercise-media", command.FileName);
        var uploadResult = await _fileProvider.UploadFile(fileData, ct);

        if (uploadResult.IsFailure)
            return uploadResult.Error;

        
        var mediaPathResult = MediaPath.Create(uploadResult.Value);
        if (mediaPathResult.IsFailure)
            return mediaPathResult.Error;

        var exercise = exerciseResult.Value;
        exercise.AddMedia(mediaPathResult.Value);

        await _exercisesRepository.SaveAsync(exercise, ct);

        return uploadResult.Value;
    }
}