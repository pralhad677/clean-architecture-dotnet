namespace Bookify.Domain.Apartments;

public record Currency
{
    internal static readonly Currency None = new("");
    public static readonly Currency Usd=new("USD");
    public static readonly Currency Eur=new("EUR");
    private Currency(string Code)
    {
        this.Code = Code;
    }
    public string Code { get; init; }

    public static Currency FromCode(string code)
    {
        return All.FirstOrDefault(c=>c.Code==code)?? throw new ApplicationException("The currency code is invalid");
    }

    public static IReadOnlyCollection<Currency> All = new[]
    {
        Usd, Eur,
    };
};