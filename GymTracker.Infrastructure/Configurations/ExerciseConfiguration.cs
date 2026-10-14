using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using GymTracker.Domain;
using GymTracker.Domain.Exercises;
using GymTracker.Domain.Exercises.ValueObjects;
namespace GymTracker.Infrastructure.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.ToTable("exercises");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(new ValueConverter<ExerciseId, Guid>(
                id => id.Value,
                value => ExerciseId.Create(value)));

        builder.Property(e => e.Name)
            .HasConversion(new ValueConverter<ExerciseName, string>(
                name => name.Value,
                value => ExerciseName.Create(value).Value))
            .IsRequired()
            .HasMaxLength(ExerciseName.MaxLength);

        
        builder.Property(e => e.MuscleGroup)
            .HasConversion(new ValueConverter<MuscleGroup, string>(
                muscleGroup => muscleGroup.Value,
                value => MuscleGroup.Create(value).Value))
            .IsRequired()
            .HasMaxLength(MuscleGroup.MaxLength);

        builder.OwnsMany(e => e.MediaPaths, tb =>
        {
            tb.ToJson("media_paths");
            tb.Property(m => m.Value).HasJsonPropertyName("value");
        });
            
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
        
        builder.HasQueryFilter(e => !EF.Property<bool>(e, "_isDeleted"));
    }
}