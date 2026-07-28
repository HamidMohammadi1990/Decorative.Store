using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Banks.Commands;

public record CreateBankResponse
{
    [JsonConverter(typeof(BankEncryptor))]
    public int Id { get; init; }
}
