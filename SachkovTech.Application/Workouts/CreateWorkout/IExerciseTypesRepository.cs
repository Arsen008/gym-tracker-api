using CSharpFunctionalExtensions;
using SachkovTech.Domain;
using SachkovTech.Domain.Exercises;
using SachkovTech.Domain.Shared.Ids;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Exercises;
namespace SachkovTech.Application.Exercises;

public interface IExerciseTypesRepository
{
    Task<ExerciseType?> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(ExerciseType exerciseType, CancellationToken cancellationToken = default);
    
    Task<ExerciseType?> GetByIdAsync(ExerciseTypeId id, CancellationToken cancellationToken = default);
}
