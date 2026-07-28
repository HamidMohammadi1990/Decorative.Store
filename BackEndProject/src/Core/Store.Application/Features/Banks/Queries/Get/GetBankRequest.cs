using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Banks.Queries;

public record GetBankRequest : IRequest<OperationResult<GetBankResponse?>>
{
    [JsonConverter(typeof(BankEncryptor))]
    public int Id { get; init; }
}
