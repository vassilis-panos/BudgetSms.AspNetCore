using BudgetSms.AspNetCore.Exceptions;
using BudgetSms.AspNetCore.Responses;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BudgetSms.AspNetCore;

public class BudgetSmsService
{
    private readonly HttpClient _httpClient;
    private readonly BudgetSmsOptions _options;

    public BudgetSmsService(
        HttpClient httpClient, 
        IOptions<BudgetSmsOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;

        if (string.IsNullOrWhiteSpace(_options.UserName))
            throw new ArgumentException("UserName is null or empty");

        if (_options.UserId is null)
            throw new ArgumentException("UserId is null");

        if (string.IsNullOrWhiteSpace(_options.Handle))
            throw new ArgumentException("Handle is null or empty");

        if (string.IsNullOrWhiteSpace(_options.Sender))
            throw new ArgumentException("Sender is null or empty");
    }

    public async Task<SendResponse> SendAsync(
        string recipientNumber, 
        string message, 
        string? messageId = default,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>()
        {
            ["username"] = _options.UserName,
            ["userid"] = Convert.ToString(_options.UserId),
            ["handle"] = _options.Handle,
            ["from"] = _options.Sender,
            ["to"] = recipientNumber,
            ["msg"] = message,
            ["customid"] = messageId,
            ["price"] = "1",
            ["mccmnc"] = "1",
            ["credit"] = "1"
        };

        return await ApiCall<SendResponse>(
           "sendsms", query, cancellationToken);
    }

    public async Task<CreditResponse> GetCreditAsync(
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>()
        {
            ["username"] = _options.UserName,
            ["userid"] = Convert.ToString(_options.UserId),
            ["handle"] = _options.Handle
        };

        return await ApiCall<CreditResponse>(
            "checkcredit", query, cancellationToken);
    }

    public async Task<StatusResponse> GetSmsStatusAsync(
        int? smsId, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>()
        {
            ["username"] = _options.UserName,
            ["userid"] = Convert.ToString(_options.UserId),
            ["handle"] = _options.Handle,
            ["smsid"] = Convert.ToString(smsId)
        };

        return await ApiCall<StatusResponse>(
            "checksms", query, cancellationToken);
    }

    private async Task<T> ApiCall<T>(
        string uri, 
        Dictionary<string, string?> query, 
        CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(
            QueryHelpers.AddQueryString(uri, query), cancellationToken);

        response.EnsureSuccessStatusCode();

        var content =
            await response.Content.ReadAsStringAsync(cancellationToken);

        if (content.StartsWith("ERR"))
            throw new ApiException(content);

        return (T)Activator.CreateInstance(typeof(T), content);
    }
}
