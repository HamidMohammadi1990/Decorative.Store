using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.RoomTypes.Queries;

public record GetAllRoomTypeRequest
    : ContentPolicyRequest<RoomType>, IRequest<OperationResult<PagedResult<GetAllRoomTypeResponse>>>
{
    public int? LanguageId { get; init; }
    public string? Title { get; init; }
    public string? Code { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}

public record GetAllRoomTypeResponse
{
    [JsonConverter(typeof(RoomTypeEncryptor))]
    public int Id { get; init; }

    public string Code { get; init; } = default!;
    public string ImageFileName { get; init; } = default!;
    public string ImageUrl { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
    public string? Title { get; init; }
    public int? LanguageId { get; init; }
}

public record GetRoomTypeRequest : IRequest<OperationResult<GetRoomTypeResponse?>>
{
    [JsonConverter(typeof(RoomTypeEncryptor))]
    public int Id { get; init; }

    public int LanguageId { get; init; }
}

public record GetRoomTypeResponse
{
    [JsonConverter(typeof(RoomTypeEncryptor))]
    public int Id { get; init; }

    public string Code { get; init; } = default!;
    public string ImageFileName { get; init; } = default!;
    public string ImageUrl { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
    public string Title { get; init; } = default!;
    public int LanguageId { get; init; }
}

public record ListRoomTypesRequest : IRequest<OperationResult<List<ListRoomTypeItemResponse>>>
{
    public int LanguageId { get; init; }
}

public record ListRoomTypeItemResponse
{
    [JsonConverter(typeof(RoomTypeEncryptor))]
    public int Id { get; init; }

    public string Code { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string ImageFileName { get; init; } = default!;
    public string ImageUrl { get; init; } = default!;
    public int Priority { get; init; }
}
