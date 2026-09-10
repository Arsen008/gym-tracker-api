using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SachkovTech.Domain;
using SachkovTech.Domain.Exercises;
using SachkovTech.Domain.Exercises.ValueObjects;
namespace SachkovTech.Infrastructure.Configurations;

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
        
        builder.Property(e => e.MediaPath)
            .HasConversion(new ValueConverter<MediaPath?, string?>(
                path => path != null ? path.Value : null,
                value => string.IsNullOrWhiteSpace(value) ? null : MediaPath.Create(value).Value))
            .HasColumnName("media_path")
            .IsRequired(false);
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
        
        builder.HasQueryFilter(e => !EF.Property<bool>(e, "_isDeleted"));
    }
}