using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.CompanyStoryLikes;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class CompanyStoryLikeRepository
    (EditionDbContext context)
    : Repository<CompanyStoryLike>(context), ICompanyStoryLikeRepository
{
    public async Task<PagedResult<CompanyStoryLikeDto>> GetAllAsync(GetAllCompanyStoryLikeRequestDto request)
    {
        var companyStoryLikeSource = Context.CompanyStoryLike
            .ApplyContentPolicyFilter(request.ContentFilter);

        var likes =
            from companyStoryLike in companyStoryLikeSource
            join companyStory in Context.CompanyStory on companyStoryLike.CompanyStoryId equals companyStory.Id
            join user in Context.User on companyStoryLike.UserId equals user.Id into joinUser
            from user in joinUser.DefaultIfEmpty()
            select new { companyStoryLike, user, companyStory };

        likes = likes.ApplyQueryFilters(request);

        var result = await likes
            .Select(x => new CompanyStoryLikeDto
            {
                Id = x.companyStoryLike.Id,
                UserName = x.user.UserName,
                ClientIP = x.companyStoryLike.ClientIP,
                CompanyStoryId = x.companyStoryLike.CompanyStoryId,
                CreatedOnUtc = x.companyStoryLike.CreatedOnUtc,
                CompanyStoryCaption = x.companyStory.Caption,
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}
