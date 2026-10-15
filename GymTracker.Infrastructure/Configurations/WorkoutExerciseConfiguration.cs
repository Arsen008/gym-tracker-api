using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GymTracker.Domain;
using GymTracker.Domain.Exercises;
using GymTracker.Domain.Shared.Ids;
using GymTracker.Domain.Workouts;
using GymTracker.Domain.Workouts.ValueObjects;

namespace GymTracker.Infrastructure.Configurations;

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

        builder.Property(we => we.ExerciseId)
            .HasConversion(
                id => id.Value,
                value => ExerciseId.Create(value));

        builder.Property(we => we.Reps)
            .HasConversion(
                reps => reps.Value,
                value => Reps.Create(value).Value);
        
        builder.Property(we => we.Sets)
            .HasConversion(
                sets => sets.Value,
                value => Sets.Create(value).Value);
        
        builder.HasOne<Exercise>()
            .WithMany()
            .HasForeignKey(we => we.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(we => we.Weight)
            .HasConversion(
                weight => weight.Value,
                value => Weight.Create(value).Value);
    }
}