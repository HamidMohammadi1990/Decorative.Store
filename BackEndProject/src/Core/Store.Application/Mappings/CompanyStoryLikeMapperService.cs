using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.CompanyStoryLikes.Queries;
using Store.Domain.Dtos.CompanyStoryLikes;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class CompanyStoryLikeMapperService : ICompanyStoryLikeMapperService
{
    public PagedResult<GetAllCompanyStoryLikeResponse> Map(PagedResult<CompanyStoryLikeDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllCompanyStoryLikeResponse
            {
                Id = x.Id,
                UserName = x.UserName,
                ClientIP = x.ClientIP,
                CompanyStoryId = x.CompanyStoryId,
                CreatedOnUtc = x.CreatedOnUtc,
                CompanyStoryCaption = x.CompanyStoryCaption
            })
            .ToList();

        return PagedResult<GetAllCompanyStoryLikeResponse>.Create(items, model);
    }

    public GetAllCompanyStoryLikeRequestDto Map(GetAllCompanyStoryLikeRequest model)
    {
        return new GetAllCompanyStoryLikeRequestDto
        {
            CompanyStoryId = model.CompanyStoryId,
            Pagination = model.Pagination
        }.WithContentPolicy<CompanyStoryLike, GetAllCompanyStoryLikeRequestDto>(model);
    }

    public GetCompanyStoryLikeResponse Map(CompanyStoryLike model)
    {
        return new GetCompanyStoryLikeResponse
        {
            Id = model.Id,
            UserId = model.UserId,
            ClientIP = model.ClientIP,
            CompanyStoryId = model.CompanyStoryId,
            CreatedOnUtc = model.CreatedOnUtc
        };
    }
}
