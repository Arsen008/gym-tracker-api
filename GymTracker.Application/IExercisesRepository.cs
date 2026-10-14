    using CSharpFunctionalExtensions;
    using GymTracker.Domain;
    using GymTracker.Domain.Exercises;
    using GymTracker.Domain.Exercises.ValueObjects;
    using GymTracker.Domain.Shared;

    namespace GymTracker.Application.Exercises;

    public interface IExercisesRepository
    {
        Task<ExerciseId> AddAsync(Exercise exercise, CancellationToken cancellationToken = default);

        Task<ExerciseId> SaveAsync(Exercise exercise, CancellationToken cancellationToken = default);

        Task<ExerciseId> DeleteAsync(Exercise exercise, CancellationToken cancellationToken = default);

        Task<Result<Exercise, Error>> GetByIdAsync(ExerciseId id, CancellationToken cancellationToken = default);

        Task<Result<Exercise, Error>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
    