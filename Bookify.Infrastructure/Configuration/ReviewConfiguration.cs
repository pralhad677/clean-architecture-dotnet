using Bookify.Domain.Review;
using Bookify.Domain.Users;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configuration;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");

        // Configure the primary key for the Review entity
        builder.HasKey(r => r.Id);

        // Configure the relationships (foreign keys)
        builder.HasOne<User>()  // Review has one User (Foreign Key)
            .WithMany()  // Assumes a User can have many Reviews
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);  // Defines cascading delete behavior (optional)

        builder.HasOne<Apartment>()  // Review has one Apartment (Foreign Key)
            .WithMany()  // Assumes an Apartment can have many Reviews
            .HasForeignKey(r => r.ApartmentId)
            .OnDelete(DeleteBehavior.Cascade);  // Defines cascading delete behavior (optional)

        builder.HasOne<Booking>()  // Review has one Booking (Foreign Key)
            .WithMany()  // Assumes a Booking can have many Reviews
            .HasForeignKey(r => r.BookingId)
            .OnDelete(DeleteBehavior.Restrict);  // Defines cascading delete behavior (optional)

        // Configure properties
        builder.Property(r => r.ApartmentId)
            .IsRequired();

        builder.Property(r => r.BookingId)
            .IsRequired();

        builder.Property(r => r.UserId)
            .IsRequired();

        builder.Property(r => r.CreatedOnUtc)
            .IsRequired();

        // Configure complex types
        builder.OwnsOne(r => r.Rating, ratingBuilder =>
        {
            ratingBuilder.Property(r => r.Value).IsRequired();
        });

        builder.OwnsOne(r => r.Comment, commentBuilder =>
        {
            commentBuilder.Property(c => c.Text).IsRequired();
        });

        // Optionally, add indexes on frequently queried fields
        builder.HasIndex(r => r.UserId);  // Index for UserId
        builder.HasIndex(r => r.ApartmentId);  // Index for ApartmentId
        builder.HasIndex(r => r.BookingId);  // Index for BookingId
    }
}
