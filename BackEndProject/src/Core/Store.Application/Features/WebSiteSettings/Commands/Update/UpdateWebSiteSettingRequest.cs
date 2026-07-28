using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.WebSiteSettings.Commands;

public record UpdateWebSiteSettingRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(WebSiteSettingEncryptor))]
    public int Id { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Telephone { get; init; }
    public string? Address { get; init; }
    public string? CartNumber { get; init; }
    public string? EmailUserName { get; init; }
    public string? EmailPassword { get; init; }
    public string? SmsAccountUrl { get; init; }
    public string? SmsAccountUserName { get; init; }
    public string? SmsAccountPassword { get; init; }
}
