using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.CompanyStoryComments.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CompanyStoryComments;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class CompanyStoryCommentMapperService : ICompanyStoryCommentMapperService
{
    public PagedResult<GetAllCompanyStoryCommentResponse> Map(PagedResult<GetAllCompanyStoryCommentResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllCompanyStoryCommentResponse
            {
                Id = x.Id,
                Content = x.Content,
                ParentId = x.ParentId,
                IsApproved = x.IsApproved,
                CompanyStoryId = x.CompanyStoryId,
                CompanyStoryCaption = x.CompanyStoryCaption,
                CreatedOnUtc = x.CreatedOnUtc,
                ApprovedOnUtc = x.ApprovedOnUtc,
                CreatedByUserFirstName = x.CreatedByUserFirstName!,
                CreatedByUserLastName = x.CreatedByUserLastName!,
                ApprovedByUserFirstName = x.ApprovedByUserFirstName,
                ApprovedByUserLastName = x.ApprovedByUserLastName,
                CreatedByUserId = x.CreatedByUserId,
                ApprovedByUserId = x.ApprovedByUserId,
            })
            .ToList();

        return PagedResult<GetAllCompanyStoryCommentResponse>.Create(items, model);
    }

    public PagedResult<SearchCompanyStoryCommentResponse> Map(PagedResult<SearchCompanyStoryCommentResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchCompanyStoryCommentResponse
            {
                Id = x.Id,
                Content = x.Content,
                ParentId = x.ParentId,
                CompanyStoryId = x.CompanyStoryId,
                CreatedOnUtc = x.CreatedOnUtc,
                ApprovedOnUtc = x.ApprovedOnUtc,
                CreatedByUserFirstName = x.CreatedByUserFirstName!,
                CreatedByUserLastName = x.CreatedByUserLastName!,
                CreatedByUserId = x.CreatedByUserId
            })
            .ToList();

        return PagedResult<SearchCompanyStoryCommentResponse>.Create(items, model);
    }

    public GetCompanyStoryCommentResponse Map(CompanyStoryComment model)
    {
        return new GetCompanyStoryCommentResponse
        {
            Id = model.Id,
            Content = model.Content,
            ParentId = model.ParentId,
            CompanyStoryId = model.CompanyStoryId,
            IsApproved = model.IsApproved,
            CreatedOnUtc = model.CreatedOnUtc,
            ApprovedOnUtc = model.ApprovedOnUtc,
            CreatedByUserId = model.CreatedByUserId,
            ApprovedByUserId = model.ApprovedByUserId
        };
    }

    public GetAllCompanyStoryCommentRequestDto Map(GetAllCompanyStoryCommentRequest model)
    {
        return new GetAllCompanyStoryCommentRequestDto
        {
            IsApproved = model.IsApproved,
            CompanyStoryId = model.CompanyStoryId,
            Pagination = model.Pagination,
            CreatedByUserId = model.CreatedByUserId,
            ApprovedByUserId = model.ApprovedByUserId
        }.WithContentPolicy<CompanyStoryComment, GetAllCompanyStoryCommentRequestDto>(model);
    }

    public SearchCompanyStoryCommentRequestDto Map(SearchCompanyStoryCommentRequest model)
    {
        return new SearchCompanyStoryCommentRequestDto
        {
            CompanyStoryId = model.CompanyStoryId,
            Pagination = model.Pagination
        }.WithContentPolicy<CompanyStoryComment, SearchCompanyStoryCommentRequestDto>(model);
    }
}
