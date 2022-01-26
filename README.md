# BudgetSms.AspNetCore [![NuGet](https://img.shields.io/nuget/v/BudgetSms.AspNetCore.svg)](https://www.nuget.org/packages/BudgetSms.AspNetCore)

ASP.NET Core library for [BudgetSMS](https://www.budgetsms.net/) gateway

## Installation

.NET CLI
```
dotnet add package BudgetSms.AspNetCore
```

Package Manager
```
Install-Package BudgetSms.AspNetCore
```

## Configuration

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddBudgetSms(options =>
        {
            options.UserName = "Vassilis";
            options.UserId = 21547;
            options.Handle = "1e756dc895456f";
            options.Sender = "Vassilis";
        });
    }
}
```

### appsettings.json

```json
{
  "BudgetSms": {
    "UserName": "Vassilis",
    "UserId": 21547,
    "Handle": "1e756dc895456f",
    "Sender": "Vassilis"
  }
}
```

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<BudgetSmsOptions>(
            Configuration.GetSection("BudgetSms"));
        services.AddBudgetSms();
    }
}
```

## Dependency injection

```csharp
public class SmsController : ControllerBase
{
    private readonly BadgetSmsService _sms;

    public SmsController(BadgetSmsService sms)
    {
        _sms = sms;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendSmsAsync(CancellationToken ct)
    {
        try
        {
            var response = await _budgetSms.SendAsync(
                "306982004055", "Test Message", cancellationToken: ct);

            return Ok(response.Id);
        }
        catch (ApiException ex)
        {
            return BadRequest(ex.ErrorDescription);
        }
    }
}
```