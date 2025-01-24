namespace Bookify.Domain.Apartments;

public record Money(decimal Amount, Currency Currency)
{
    public static Money operator +(Money a, Money b)
    {
        if (a.Currency.Code != b.Currency.Code)
        {
            throw new ApplicationException("Currency code mismatch");
        }
        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money Zero() => new(0, Currency.None);
    public static Money Zero(Currency currency) => new(0,  currency );
    public bool IsZero() => this ==   Zero(Currency );
};