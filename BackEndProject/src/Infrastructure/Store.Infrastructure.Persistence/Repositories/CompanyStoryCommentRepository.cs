using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CompanyStoryComments;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class CompanyStoryCommentRepository
    (EditionDbContext context)
    : Repository<CompanyStoryComment>(context), ICompanyStoryCommentRepository
{
    public async Task<PagedResult<GetAllCompanyStoryCommentResponseDto>> GetAllAsync(GetAllCompanyStoryCommentRequestDto request)
    {
        var companyStoryCommentSource = Context.CompanyStoryComment
            .ApplyContentPolicyFilter(request.ContentFilter);

        var comments =
            from companyStoryComment in companyStoryCommentSource
            join companyStory in Context.CompanyStory on companyStoryComment.CompanyStoryId equals companyStory.Id
            join createdByUser in Context.User on companyStoryComment.CreatedByUserId equals createdByUser.Id
            join approvedByUser in Context.User on companyStoryComment.ApprovedByUserId equals approvedByUser.Id into joinApprovedByUser
            from approvedByUser in joinApprovedByUser.DefaultIfEmpty()
            select new { companyStoryComment, companyStory, createdByUser, approvedByUser };

        comments = comments.ApplyQueryFilters(request);

        var result = await comments
            .Select(x => new GetAllCompanyStoryCommentResponseDto
            {
                Id = x.companyStoryComment.Id,
                Content = x.companyStoryComment.Content,
                ParentId = x.companyStoryComment.ParentId,
                IsApproved = x.companyStoryComment.IsApproved,
                CompanyStoryId = x.companyStoryComment.CompanyStoryId,
                CompanyStoryCaption = x.companyStory.Caption,
                CreatedOnUtc = x.companyStoryComment.CreatedOnUtc,
                ApprovedOnUtc = x.companyStoryComment.ApprovedOnUtc,
                CreatedByUserFirstName = x.createdByUser.FirstName,
                CreatedByUserLastName = x.createdByUser.LastName,
                ApprovedByUserFirstName = x.approvedByUser.FirstName,
                ApprovedByUserLastName = x.approvedByUser.LastName,
                CreatedByUserId = x.companyStoryComment.CreatedByUserId,
                ApprovedByUserId = x.companyStoryComment.ApprovedByUserId,
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchCompanyStoryCommentResponseDto>> SearchAsync(SearchCompanyStoryCommentRequestDto request)
    {
        var companyStoryCommentSource = Context.CompanyStoryComment
            .ApplyContentPolicyFilter(request.ContentFilter);

        var comments =
            from companyStoryComment in companyStoryCommentSource
            join createdByUser in Context.User on companyStoryComment.CreatedByUserId equals createdByUser.Id
            where companyStoryComment.IsApproved
            select new { companyStoryComment, createdByUser };

        comments = comments.ApplyQueryFilters(request);

        var result = await comments
            .Select(x => new SearchCompanyStoryCommentResponseDto
            {
                Id = x.companyStoryComment.Id,
                Content = x.companyStoryComment.Content,
                ParentId = x.companyStoryComment.ParentId,
                CompanyStoryId = x.companyStoryComment.CompanyStoryId,
                CreatedOnUtc = x.companyStoryComment.CreatedOnUtc,
                ApprovedOnUtc = x.companyStoryComment.ApprovedOnUtc,
                CreatedByUserFirstName = x.createdByUser.FirstName,
                CreatedByUserLastName = x.createdByUser.LastName,
                CreatedByUserId = x.companyStoryComment.CreatedByUserId
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}
