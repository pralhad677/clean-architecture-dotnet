namespace Bookify.Domain.Review;

public class Comment
{
    public string Text { get; private set; }

    // Constructor ensuring the comment text is within a valid length
    public Comment(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Comment cannot be empty.", nameof(text));
        }

        if (text.Length > 1000) // Assuming a max length of 1000 characters
        {
            throw new ArgumentOutOfRangeException(nameof(text), "Comment cannot exceed 1000 characters.");
        }

        Text = text;
    }
}