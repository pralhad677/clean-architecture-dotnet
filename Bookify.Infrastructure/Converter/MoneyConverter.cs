using Bookify.Domain.Apartments;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bookify.Infrastructure.Converters
{
    public class MoneyConverter : ValueConverter<Money, string>
    {
        public MoneyConverter() : base(
            money => ConvertToString(money),
            str => ConvertToMoney(str))
        {
        }

        private static string ConvertToString(Money money)
        {
            return $"{money.Amount}:{money.Currency.Code}";
        }

        private static Money ConvertToMoney(string str)
        {
            var parts = str.Split(':');
            var amount = decimal.Parse(parts[0]);
            var currency = Currency.FromCode(parts[1]);
            return new Money(amount, currency);
        }
    }
}