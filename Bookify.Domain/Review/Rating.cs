namespace Bookify.Domain.Review;

public class Rating
{
    public int Value { get; private set; }

    // Constructor ensuring the rating value is within the valid range
    public Rating(int value)
    {
        if (value < 1 || value > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Rating must be between 1 and 5.");
        }

        Value = value;
    }
}