using BudgetSms.AspNetCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class BudgetSmsExtensions
{
    public static IServiceCollection AddBudgetSms(
        this IServiceCollection services, Action<BudgetSmsOptions> configure)
    {
        services.AddBudgetSms();
        services.Configure(configure);
        return services;
    }

    public static IServiceCollection AddBudgetSms(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddHttpClient<BudgetSmsService>(options =>
        {
            options.BaseAddress = new Uri("https://api.budgetsms.net/");
        });

        services.TryAddScoped<BudgetSmsService>();

        return services;
    }
}
