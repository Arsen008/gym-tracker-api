    using CSharpFunctionalExtensions;
    using SachkovTech.Domain;
    using SachkovTech.Domain.Exercises;
    using SachkovTech.Domain.Exercises.ValueObjects;
    using SachkovTech.Domain.Shared;

    namespace SachkovTech.Application.Exercises;

    public interface IExercisesRepository
    {
        Task<ExerciseId> AddAsync(Exercise exercise, CancellationToken cancellationToken = default);

        Task<ExerciseId> SaveAsync(Exercise exercise, CancellationToken cancellationToken = default);

        Task<ExerciseId> DeleteAsync(Exercise exercise, CancellationToken cancellationToken = default);

        Task<Result<Exercise, Error>> GetByIdAsync(ExerciseId id, CancellationToken cancellationToken = default);

        Task<Result<Exercise, Error>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
    