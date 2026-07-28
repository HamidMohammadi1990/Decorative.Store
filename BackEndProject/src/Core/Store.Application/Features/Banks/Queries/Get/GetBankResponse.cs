using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Banks.Queries;

public record GetBankResponse
{
    [JsonConverter(typeof(BankEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string Icon { get; init; } = default!;
    public bool IsActive { get; init; }
}
