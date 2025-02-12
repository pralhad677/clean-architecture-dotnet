using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Abstraction.Messaging;
using Bookify.Application.Exception;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;

namespace Bookify.Application.Bookings.ReserveBooking;

internal sealed  class ReserveBookingCommandHandler:ICommandHandler<ReserveBookingCommand,Guid>
{
     


    private readonly IUserRepository _repository;
    private readonly IApartmentsRepository apartmentsRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PricingService _pricingService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ReserveBookingCommandHandler(
        IUserRepository repository,
        IApartmentsRepository apartmentsRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        PricingService pricingService,
        IDateTimeProvider dateTimeProvider)
    {
        _repository = repository;
        this.apartmentsRepository = apartmentsRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _pricingService = pricingService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        var apartment = await apartmentsRepository.GetByIdAsync(request.ApartmentId, cancellationToken);
        if (apartment is null)
        {
            return Result.Failure<Guid>(ApartmentErrors.NotFound);
        }

        var duration = DateRange.Create(request.StartDate, request.EndDate);
        if (await _bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken))
        {
            return Result.Failure<Guid>(BookingErrors.NotFound);
        }

        try
        {
            var booking = Booking.Reserve(
                apartment,
                user.Id,
                duration,
                _dateTimeProvider.UtcNow,
                _pricingService
            );

            _bookingRepository.Add(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return booking.Id;
        }
        catch (ConcurrencyException ex)
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }
    }

}