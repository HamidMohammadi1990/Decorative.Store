using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Domain.Repositories;
using Store.Domain.Dtos.UserStoryComments;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class UserStoryCommentRepository
    (EditionDbContext context)
    : Repository<UserStoryComment>(context), IUserStoryCommentRepository
{
    public async Task<PagedResult<GetAllUserStoryCommentResponseDto>> GetAllAsync(GetAllUserStoryCommentRequestDto request)
    {
        var commentSource = Context.UserStoryComment
            .ApplyContentPolicyFilter(request.ContentFilter);

        var comments =
            from userStoryComment in commentSource
            join userStory in Context.UserStory on userStoryComment.UserStoryId equals userStory.Id
            join user in Context.User on userStoryComment.UserId equals user.Id
            join approvedByUser in Context.User on userStoryComment.ApprovedByUserId equals approvedByUser.Id into joinApprovedByUser
            from approvedByUser in joinApprovedByUser.DefaultIfEmpty()
            select new { userStoryComment, userStory, user, approvedByUser };

        comments = comments.ApplyQueryFilters(request);

        return await comments
            .Select(x => new GetAllUserStoryCommentResponseDto
            {
                Id = x.userStoryComment.Id,
                Content = x.userStoryComment.Content,
                IsApproved = x.userStoryComment.IsApproved,
                UserStoryId = x.userStoryComment.UserStoryId,
                UserStoryTitle = x.userStory.Title,
                CreatedOnUtc = x.userStoryComment.CreatedOnUtc,
                ApprovedOnUtc = x.userStoryComment.ApprovedOnUtc,
                UserId = x.userStoryComment.UserId,
                UserFirstName = x.user.FirstName,
                UserLastName = x.user.LastName,
                ApprovedByUserId = x.userStoryComment.ApprovedByUserId,
                ApprovedByUserFirstName = x.approvedByUser.FirstName,
                ApprovedByUserLastName = x.approvedByUser.LastName,
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<SearchUserStoryCommentResponseDto>> SearchAsync(SearchUserStoryCommentRequestDto request)
    {
        var commentSource = Context.UserStoryComment
            .ApplyContentPolicyFilter(request.ContentFilter);

        var comments =
            from userStoryComment in commentSource
            join user in Context.User on userStoryComment.UserId equals user.Id
            where userStoryComment.IsApproved
            select new { userStoryComment, user };

        comments = comments.ApplyQueryFilters(request);

        return await comments
            .Select(x => new SearchUserStoryCommentResponseDto
            {
                Id = x.userStoryComment.Id,
                Content = x.userStoryComment.Content,
                UserStoryId = x.userStoryComment.UserStoryId,
                CreatedOnUtc = x.userStoryComment.CreatedOnUtc,
                ApprovedOnUtc = x.userStoryComment.ApprovedOnUtc,
                UserId = x.userStoryComment.UserId,
                UserFirstName = x.user.FirstName,
                UserLastName = x.user.LastName,
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }

    public async Task<Dictionary<int, int>> GetApprovedCommentCountsByStoryIdsAsync(
        IReadOnlyCollection<int> storyIds,
        CancellationToken cancellationToken = default)
    {
        if (storyIds.Count == 0)
            return new Dictionary<int, int>();

        var counts = await Context.UserStoryComment
            .AsNoTracking()
            .Where(x => storyIds.Contains(x.UserStoryId) && x.IsApproved)
            .GroupBy(x => x.UserStoryId)
            .Select(g => new { UserStoryId = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        return counts.ToDictionary(x => x.UserStoryId, x => x.Count);
    }
}
