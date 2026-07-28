using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Banks.Commands;

public record DeleteBankRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(BankEncryptor))]
    public int Id { get; init; }
}
