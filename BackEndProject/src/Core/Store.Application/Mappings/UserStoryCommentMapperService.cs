using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.UserStoryComments.Queries;
using Store.Domain.Dtos.UserStoryComments;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Mappings;

public class UserStoryCommentMapperService : IUserStoryCommentMapperService
{
    public PagedResult<GetAllUserStoryCommentResponse> Map(PagedResult<GetAllUserStoryCommentResponseDto> model)
    {
        var items = model.Items
            .Select(x => new GetAllUserStoryCommentResponse
            {
                Id = x.Id,
                Content = x.Content,
                IsApproved = x.IsApproved,
                UserStoryId = x.UserStoryId,
                UserStoryTitle = x.UserStoryTitle,
                CreatedOnUtc = x.CreatedOnUtc,
                ApprovedOnUtc = x.ApprovedOnUtc,
                UserId = x.UserId,
                UserFirstName = x.UserFirstName ?? string.Empty,
                UserLastName = x.UserLastName ?? string.Empty,
                ApprovedByUserId = x.ApprovedByUserId,
                ApprovedByUserFirstName = x.ApprovedByUserFirstName,
                ApprovedByUserLastName = x.ApprovedByUserLastName,
            })
            .ToList();

        return PagedResult<GetAllUserStoryCommentResponse>.Create(items, model);
    }

    public PagedResult<SearchUserStoryCommentResponse> Map(PagedResult<SearchUserStoryCommentResponseDto> model)
    {
        var items = model.Items
            .Select(x => new SearchUserStoryCommentResponse
            {
                Id = x.Id,
                Content = x.Content,
                UserStoryId = x.UserStoryId,
                CreatedOnUtc = x.CreatedOnUtc,
                ApprovedOnUtc = x.ApprovedOnUtc,
                UserId = x.UserId,
                UserFirstName = x.UserFirstName ?? string.Empty,
                UserLastName = x.UserLastName ?? string.Empty,
            })
            .ToList();

        return PagedResult<SearchUserStoryCommentResponse>.Create(items, model);
    }

    public GetAllUserStoryCommentRequestDto Map(GetAllUserStoryCommentRequest model)
        => new()
        {
            UserStoryId = model.UserStoryId,
            UserId = model.UserId,
            IsApproved = model.IsApproved,
            Pagination = model.Pagination,
        };

    public SearchUserStoryCommentRequestDto Map(SearchUserStoryCommentRequest model)
        => new()
        {
            UserStoryId = model.UserStoryId,
            Pagination = model.Pagination,
        };
}
