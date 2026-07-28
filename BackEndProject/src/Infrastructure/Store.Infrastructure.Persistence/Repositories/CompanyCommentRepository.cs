using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.CompanyComments;
using Store.Domain.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Store.Infrastructure.Persistence.Repositories;

public class CompanyCommentRepository
    (EditionDbContext context)
    : Repository<CompanyComment>(context), ICompanyCommentRepository
{
    public async Task<PagedResult<GetAllCompanyCommentResponseDto>> GetAllAsync(GetUserCompanyCommentRequestDto request)
    {
        var companyCommentSource = Context.CompanyComment
            .ApplyContentPolicyFilter(request.ContentFilter);

        var query =
            from companyComment in companyCommentSource
            join company in Context.Company on companyComment.CompanyId equals company.Id
            join user in Context.User on companyComment.UserId equals user.Id
            select new { companyComment, company, user };

        query = query.ApplyQueryFilters(request);

        var result = await
            query
            .OrderByDescending(x => x.companyComment.CreatedOnUtc)
            .Select(x => new GetAllCompanyCommentResponseDto
            {
                Id = x.companyComment.Id,
                Rate = x.companyComment.Rate,
                Title = x.companyComment.Title,
                UserId = x.companyComment.UserId,
                UserName = x.user.UserName,
                UserFirstName = x.user.FirstName,
                UserLastName = x.user.LastName,
                CompanyId = x.companyComment.CompanyId,
                StatusType = x.companyComment.StatusType,
                Description = x.companyComment.Description,
                CompanyName = x.company.Name,
                CreatedOnUtc = x.companyComment.CreatedOnUtc
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchCompanyCommentResponseDto>> SearchAsync(SearchCompanyCommentRequestDto request)
    {
        var companyCommentSource = Context.CompanyComment
            .ApplyContentPolicyFilter(request.ContentFilter);

        var query =
            from companyComment in companyCommentSource
            join company in Context.Company on companyComment.CompanyId equals company.Id
            join user in Context.User on companyComment.UserId equals user.Id
            where companyComment.StatusType == CommentStatusType.Approved
            select new { companyComment, company, user };

        query = query.ApplyQueryFilters(request);

        var result = await
            query
            .OrderByDescending(x => x.companyComment.CreatedOnUtc)
            .Select(x => new SearchCompanyCommentResponseDto
            {
                Id = x.companyComment.Id,
                Rate = x.companyComment.Rate,
                Title = x.companyComment.Title,
                UserId = x.companyComment.UserId,
                UserName = x.user.UserName,
                UserFirstName = x.user.FirstName,
                UserLastName = x.user.LastName,
                CompanyId = x.companyComment.CompanyId,
                Description = x.companyComment.Description,
                CompanyName = x.company.Name,
                CreatedOnUtc = x.companyComment.CreatedOnUtc
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}