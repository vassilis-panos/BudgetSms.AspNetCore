using System.Globalization;
using System.Linq;

namespace BudgetSms.AspNetCore.Responses;

public class CreditResponse
{
    public decimal? Credit { get; }

    public CreditResponse(string successText)
    {
        var value = successText
            .Split()
            .LastOrDefault();

        if (decimal.TryParse(value, NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture, out var credit))
        {
            Credit = credit;
        }
    }
}
