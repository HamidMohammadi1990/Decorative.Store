using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.RoomTypes.Commands;

public record CreateRoomTypeRequest : IRequest<OperationResult<CreateRoomTypeResponse>>
{
    public int LanguageId { get; init; }
    public string Code { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string ImageFileName { get; init; } = default!;
    public int Priority { get; init; }
}

public record CreateRoomTypeResponse
{
    [JsonConverter(typeof(RoomTypeEncryptor))]
    public int Id { get; init; }
}

public record UpdateRoomTypeRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(RoomTypeEncryptor))]
    public int Id { get; init; }

    public int LanguageId { get; init; }
    public string Code { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string ImageFileName { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}

public record DeleteRoomTypeRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(RoomTypeEncryptor))]
    public int Id { get; init; }
}

public record UploadRoomTypeImageResponse
{
    public string ImageFileName { get; init; } = default!;
    public string ImageUrl { get; init; } = default!;
}
