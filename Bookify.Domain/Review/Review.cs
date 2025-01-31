using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Review;

public sealed class Review:Entity
{
    public Review()
    {

    }

    public Review(
        Guid id,
        Guid apartmentId,
        Guid  bookingId,
        Guid userId,
        Rating rating,
        Comment comment,
        DateTime createdOnUtc )

    {
    this.Id = id;
    this.ApartmentId = apartmentId;
    this.BookingId = bookingId;
    this.UserId = userId;
    this.CreatedOnUtc= createdOnUtc;
    }

    public Guid ApartmentId { get; set; }

    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public DateTime?  CreatedOnUtc { get; set; }
    public Rating Rating { get; set; }
    public Comment Comment { get; set; }

}