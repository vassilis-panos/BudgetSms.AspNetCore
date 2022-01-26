using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSms.AspNetCore;

public class BudgetSmsOptions
{
    public string? UserName { get; set; }
    public int? UserId { get; set; }
    public string? Handle { get; set; }
    public string? Sender { get; set; }
}
