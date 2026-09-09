using Microsoft.EntityFrameworkCore;
using SachkovTech.Application.Exercises;
using SachkovTech.Domain;
using SachkovTech.Domain.Exercises;
using SachkovTech.Infrastructure;
using SachkovTech.Domain.Shared.Ids;
namespace SachkovTech.Infrastructure.Repositories;

public class ExerciseTypesRepository : IExerciseTypesRepository
{
    private readonly ApplicationDbContext _context;

    public ExerciseTypesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExerciseType?> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _context.ExerciseTypes
            .Include(et => et.Exercises)
            .FirstOrDefaultAsync(et => et.Title.ToLower() == title.ToLower(), cancellationToken);
    }

    public async Task<Guid> AddAsync(ExerciseType exerciseType, CancellationToken cancellationToken = default)
    {
        await _context.ExerciseTypes.AddAsync(exerciseType, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return exerciseType.Id.Value;
    }
    
    public async Task<ExerciseType?> GetByIdAsync(ExerciseTypeId id, CancellationToken cancellationToken = default)
    {
        return await _context.ExerciseTypes
            .Include(et => et.Exercises) // <--- Добавляем подгрузку связанных упражнений
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}