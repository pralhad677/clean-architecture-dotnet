using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using Bookify.Domain.Apartments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bookify.Infrastructure.Converters;

namespace Bookify.Infrastructure.Configuration
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("bookings");

            // Configure the primary key for the Booking entity
            builder.HasKey(b => b.Id);

            // Configure the relationships (foreign keys)
            builder.HasOne<User>()  // Booking has one User (Foreign Key)
                .WithMany()  // Assumes a User can have many Bookings
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Defines cascading delete behavior (optional)

            builder.HasOne<Apartment>()  // Booking has one Apartment (Foreign Key)
                .WithMany()  // Assumes an Apartment can have many Bookings
                .HasForeignKey(b => b.ApartmentId)
                .OnDelete(DeleteBehavior.Restrict);  // Defines cascading delete behavior (optional)

            // Configure properties
            builder.Property(b => b.ApartmentId).IsRequired();
            builder.Property(b => b.UserId).IsRequired();

            builder.OwnsOne(b => b.Duration, durationBuilder =>
            {
                durationBuilder.Property(d => d.Start).IsRequired();
                durationBuilder.Property(d => d.End).IsRequired();
            });

            // Apply the MoneyConverter to Money properties
            var moneyConverter = new MoneyConverter();

            builder.Property(b => b.PriceForPeriod)
                .IsRequired()
                .HasConversion(moneyConverter);

            builder.Property(b => b.CleaningFee)
                .IsRequired()
                .HasConversion(moneyConverter);

            builder.Property(b => b.AmenitiesUpCharge)
                .IsRequired()
                .HasConversion(moneyConverter);

            builder.Property(b => b.TotalPrice)
                .IsRequired()
                .HasConversion(moneyConverter);

            builder.Property(b => b.Status).IsRequired();  // Assuming BookingStatus is an enum
            builder.Property(b => b.CreatedOnUtc).IsRequired();
            builder.Property(b => b.ConfirmedOnUtc).IsRequired(false);  // Nullable property
            builder.Property(b => b.RejectedOnUtc).IsRequired(false);  // Nullable property
            builder.Property(b => b.CompletedOnUtc).IsRequired(false);  // Nullable property
            builder.Property(b => b.CanceledOnUtc).IsRequired(false);  // Nullable property

            // Optionally, add indexes on frequently queried fields
            builder.HasIndex(b => b.UserId);  // Index for UserId
            builder.HasIndex(b => b.ApartmentId);  // Index for ApartmentId
        }
    }
}
