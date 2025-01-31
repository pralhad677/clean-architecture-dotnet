using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        // Configure the primary key for the User entity
        builder.HasKey(u => u.Id);

        // Configure FirstName property
        builder.Property(u => u.Firstname)
            .IsRequired()  // Makes the FirstName required
            .HasMaxLength(100)
               .HasConversion(description=>description.value,value=>new FirstName(value)); ;  // Sets a maximum length for FirstName

        // Configure LastName property
        builder.Property(u => u.Lastname)
            .IsRequired()  // Makes the LastName required
            .HasMaxLength(100)
            .HasConversion(description=>description.value,value=>new LastName(value)); ;  // Sets a maximum length for LastName

        // Configure Email property
        builder.Property(u => u.Email)
            .IsRequired()  // Makes the Email required
            .HasMaxLength(255)
            .HasConversion(description=>description.value,value=>new Domain.Users.Email(value)); ;  // Sets a maximum length for Email

        // Optionally, you can configure a unique constraint for Email
        builder.HasIndex(u => u.Email)
            .IsUnique();

        // You can also configure additional behavior, such as setting a default value, etc.
    }
}