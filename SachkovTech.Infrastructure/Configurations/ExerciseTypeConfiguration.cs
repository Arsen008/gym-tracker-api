using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Domain.Exercises;
using SachkovTech.Domain.Shared.Ids; // <--- Вот этот using решает проблему с ExerciseTypeId

namespace SachkovTech.Infrastructure.Configurations;

public class ExerciseTypeConfiguration : IEntityTypeConfiguration<ExerciseType>
{
    public void Configure(EntityTypeBuilder<ExerciseType> builder)
    {
        builder.ToTable("exercise_types");
        
        // Первичный ключ — это Strongly-Typed ID (по канону DDD)
        builder.HasKey(et => et.Id);
        
        builder.Property(et => et.Id)
            .HasConversion(
                id => id.Value,
                value => ExerciseTypeId.Create(value));
        
        builder.Property(et => et.Title)
            .IsRequired()
            .HasMaxLength(ExerciseType.MAX_TITLE_LENGTH);
        
        builder.HasMany(et => et.Exercises)
            .WithOne()
            .HasForeignKey("ExerciseTypeId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Navigation(et => et.Exercises)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}