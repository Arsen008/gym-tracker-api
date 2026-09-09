using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Domain;
using SachkovTech.Domain.Shared.Ids;
using SachkovTech.Domain.Workouts;
using SachkovTech.Domain.Workouts.ValueObjects;

namespace SachkovTech.Infrastructure.Configurations;

public class WorkoutExerciseConfiguration : IEntityTypeConfiguration<WorkoutExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutExercise> builder)
    {
        builder.ToTable("workout_exercises");
        builder.HasKey(we => we.Id);
        
        builder.Property(we => we.Id)
            .HasConversion(
                id => id.Value,
                value => WorkoutExerciseId.Create(value));

        builder.Property<WorkoutId>("WorkoutId")
            .HasConversion(
                id => id.Value,
                value => WorkoutId.Create(value));

         
        builder.ComplexProperty(we => we.ExerciseType, tb =>
        {
            tb.Property(p => p.ExerciseTypeId)
                .HasColumnName("exercise_type_id")
                .HasConversion(
                    id => id.Value,
                    value => ExerciseTypeId.Create(value));

            tb.Property(p => p.ExerciseId)
                .HasColumnName("exercise_id");
        });

        builder.Property(we => we.Reps)
            .HasConversion(
                reps => reps.Value,
                value => Reps.Create(value).Value);
        
        builder.Property(we => we.Sets)
            .HasConversion(
                sets => sets.Value,
                value => Sets.Create(value).Value);
        
        builder.Property(we => we.Weight)
            .HasConversion(
                weight => weight.Value,
                value => Weight.Create(value).Value);
    }
}