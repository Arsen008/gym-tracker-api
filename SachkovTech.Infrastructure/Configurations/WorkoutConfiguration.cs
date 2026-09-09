using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Domain;

namespace SachkovTech.Infrastructure.Configurations;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.ToTable("workouts")
            .HasKey(w => w.Id);
        builder.Property(w => w.Id)
            .HasConversion(
                id => id.Value, 
                value =>   WorkoutId.Create(value));
        
        builder.HasMany(W => W.Exercises)
            .WithOne()
            .HasForeignKey("WorkoutId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Navigation(w => w.Exercises)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsOne(w => w.Tags, tb =>
        {
            tb.ToJson("tags");

            tb.OwnsMany(t => t.Items, itemBuilder =>
            {
                itemBuilder.Property(i => i.Value)
                    .IsRequired()
                    .HasMaxLength(30);
            });
        });
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        builder.HasQueryFilter(w => !EF.Property<bool>(w, "_isDeleted"));
    }
}