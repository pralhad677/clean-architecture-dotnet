using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Users.Events;

namespace Bookify.Domain.Bookings;

public sealed class Booking : Entity
{
    // Parameterless constructor required by EF Core
    private Booking()
    {
    }

    private Booking(
        Guid id,
        Guid apartmentId,
        Guid userId,
        DateRange duration,
        Money priceForPeriod,
        Money cleaningFee,
        Money amenitiesUpCharge,
        Money totalPrice,
        BookingStatus status,
        DateTime createdOnUtc
    ) : base(id)
    {
        ApartmentId = apartmentId;
        UserId = userId;
        Duration = duration;
        PriceForPeriod = priceForPeriod;
        CleaningFee = cleaningFee;
        AmenitiesUpCharge = amenitiesUpCharge;
        TotalPrice = totalPrice;
        Status = status;
        CreatedOnUtc = createdOnUtc;
    }

    public Guid ApartmentId { get; set; }
    public Guid UserId { get; set; }
    public DateRange Duration { get; set; }
    public Money PriceForPeriod { get; set; }
    public Money CleaningFee { get; set; }
    public Money AmenitiesUpCharge { get; set; }
    public Money TotalPrice { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ConfirmedOnUtc { get; set; }
    public DateTime? RejectedOnUtc { get; set; }
    public DateTime? CompletedOnUtc { get; set; }
    public DateTime? CanceledOnUtc { get; set; }

    public static Booking Reserve(
        Apartment apartment,
        Guid userId,
        DateRange duration,
        DateTime utcNow,
        PricingService pricingService
    )
    {
        var pricingDetails = pricingService.CalculatePrice(apartment, duration);
        var booking = new Booking(
            Guid.NewGuid(),
            apartment.Id,
            userId,
            duration,
            pricingDetails.PriceForPeriod,
            pricingDetails.CleaningFee,
            pricingDetails.AmenitiesUpCharge,
            pricingDetails.TotalPrice,
            BookingStatus.Reserved,
            utcNow
        );

        booking.RaiseDomainEvents(new BookingDomainEvents(booking.Id));

        apartment.LastBookedOnTuc = utcNow;
        return booking;
    }

    public Result Confirm(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotReserved);
        }

        Status = BookingStatus.Confirmed;
        ConfirmedOnUtc = utcNow;
        RaiseDomainEvents(new BookingConfirmedDomainEvent(Id));
        return Result.Success();
    }

    public Result Reject(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotReserved);
        }

        Status = BookingStatus.Rejected;
        RejectedOnUtc = utcNow;
        RaiseDomainEvents(new BookingRejectedDomainEvent(Id));
        return Result.Success();
    }

    public Result Complete(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotReserved);
        }

        Status = BookingStatus.Completed;
        RejectedOnUtc = utcNow;
        RaiseDomainEvents(new BookingCompletedDomainEvent(Id));
        return Result.Success();
    }

    public Result Cancel(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotReserved);
        }

        var currentDate = DateOnly.FromDateTime(utcNow);
        if (currentDate > Duration.Start)
        {
            return Result.Failure(BookingErrors.AlreadyStarted);
        }

        Status = BookingStatus.Canceled;
        RejectedOnUtc = utcNow;
        RaiseDomainEvents(new BookingCanceledDomainEvent(Id));
        return Result.Success();
    }
}
