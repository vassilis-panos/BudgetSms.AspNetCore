using System.Globalization;

namespace BudgetSms.AspNetCore.Responses;

public class SendResponse
{
    public int? Id { get; }
    public decimal? Cost { get; }
    public int? Parts { get; }
    public string? MccMnc { get; }
    public decimal? Credit { get; }

    public SendResponse(string successText)
    {
        var values = successText.Split();

        if (values.Length < 6)
            return;

        if (int.TryParse(values[1], out var id))
            Id = id;

        if (decimal.TryParse(values[2], NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture, out var cost))
        {
            Cost = cost;
        }

        if (int.TryParse(values[3], out var parts))
            Parts = parts;

        MccMnc = values[4];

        if (decimal.TryParse(values[5], NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture, out var credit))
        {
            Credit = credit;
        }
    }
}
