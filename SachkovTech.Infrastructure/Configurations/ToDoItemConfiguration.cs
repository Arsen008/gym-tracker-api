namespace SachkovTech.Infrastructure.Configurations;
 
 using Microsoft.EntityFrameworkCore;
 using Microsoft.EntityFrameworkCore.Metadata.Builders;
 using SachkovTech.Domain;
 
 public class ToDoItemConfiguration : IEntityTypeConfiguration<ToDoItem>
 {
     public void Configure(EntityTypeBuilder<ToDoItem> builder)
     {
         builder.ToTable("todo_items");
 
         builder.HasKey(x => x.Id);
 
         builder.Property(x => x.Id)
             .HasConversion(
                 id => id.Value,
                 value => ToDoItemId.Create(value));
 
         builder.Property(x => x.Title)
             .IsRequired()
             .HasMaxLength(ToDoItem.MAX_TITLE_LENGTH);
 
         builder.Property(x => x.CreatedAt)
             .IsRequired();
     }
 }