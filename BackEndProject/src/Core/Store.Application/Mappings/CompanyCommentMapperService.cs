using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.CompanyComments.Queries;
using Store.Domain.Dtos.CompanyComments;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class CompanyCommentMapperService : ICompanyCommentMapperService
{
    public GetCompanyCommentResponse Map(CompanyComment model)
    {
        return new GetCompanyCommentResponse
        {
            Id = model.Id,
            Rate = model.Rate,
            Title = model.Title,
            UserId = model.UserId,
            ParentId = model.ParentId,
            CompanyId = model.CompanyId,
            StatusType = model.StatusType,
            Description = model.Description,
            CreatedOnUtc = model.CreatedOnUtc
        };
    }

    public GetUserCompanyCommentRequestDto Map(GetAllCompanyCommentRequest model)
    {
        return new GetUserCompanyCommentRequestDto
        {
            UserId = model.UserId,
            Status = model.Status,
            CompanyId = model.CompanyId,
            Pagination = model.Pagination,
        }.WithContentPolicy<CompanyComment, GetUserCompanyCommentRequestDto>(model);
    }

    public SearchCompanyCommentRequestDto Map(SearchCompanyCommentRequest model)
    {
        return new SearchCompanyCommentRequestDto
        {
            CompanyId = model.CompanyId,
            Pagination = model.Pagination
        }.WithContentPolicy<CompanyComment, SearchCompanyCommentRequestDto>(model);
    }

    public PagedResult<GetAllCompanyCommentResponse> Map(PagedResult<GetAllCompanyCommentResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllCompanyCommentResponse
            {
                Id = x.Id,
                Rate = x.Rate,
                Title = x.Title,
                UserId = x.UserId,
                UserName = x.UserName,
                UserFirstName = x.UserFirstName,
                UserLastName = x.UserLastName,
                CompanyId = x.CompanyId,
                StatusType = x.StatusType,
                Description = x.Description,
                CompanyName = x.CompanyName,
                CreatedOnUtc = x.CreatedOnUtc
            })
            .ToList();

        return PagedResult<GetAllCompanyCommentResponse>.Create(items, model);
    }

    public PagedResult<SearchCompanyCommentResponse> Map(PagedResult<SearchCompanyCommentResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchCompanyCommentResponse
            {
                Id = x.Id,
                Rate = x.Rate,
                Title = x.Title,
                UserId = x.UserId,
                UserName = x.UserName,
                UserFirstName = x.UserFirstName,
                UserLastName = x.UserLastName,
                CompanyId = x.CompanyId,
                Description = x.Description,
                CompanyName = x.CompanyName,
                CreatedOnUtc = x.CreatedOnUtc
            })
            .ToList();

        return PagedResult<SearchCompanyCommentResponse>.Create(items, model);
    }
}