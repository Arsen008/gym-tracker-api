using CSharpFunctionalExtensions;
using SachkovTech.Application.Providers;
using SachkovTech.Domain;
using SachkovTech.Domain.Exercises;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Exercises.DeleteExerciseMedia;

public class DeleteExerciseMediaHandler(
    IExercisesRepository repository,
    IFileProvider fileProvider)
{
    private const string BUCKET_NAME = "exercise-media";

    public async Task<UnitResult<Error>> Handle(
        DeleteExerciseMediaCommand command,
        CancellationToken cancellationToken = default)
    {
        var exerciseResult = await repository.GetByIdAsync(ExerciseId.Create(command.ExerciseId), cancellationToken);
        if (exerciseResult.IsFailure)
            return exerciseResult.Error;

        var exercise = exerciseResult.Value;

        if (exercise.MediaPath == null)
            return Result.Success<Error>();

      
        var removeResult = await fileProvider.RemoveFile(
            BUCKET_NAME,
            exercise.MediaPath.Value,
            cancellationToken);

        if (removeResult.IsFailure)
            return removeResult.Error;

         
        exercise.ClearMedia();

         
        await repository.SaveAsync(exercise, cancellationToken);

        return Result.Success<Error>();
    }
}