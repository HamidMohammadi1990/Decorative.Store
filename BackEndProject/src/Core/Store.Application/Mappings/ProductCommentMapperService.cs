using Edition.Application.Common.Directories;
using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.ProductComments.Queries;
using Store.Domain.Dtos.ProductComments;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ProductCommentMapperService : IProductCommentMapperService
{
    public GetProductCommentResponse Map(ProductComment model)
    {
        return new GetProductCommentResponse
        {
            Id = model.Id,
            UserId = model.UserId,
            ProductId = model.ProductId,
            CommentRate = model.CommentRate,
            Description = model.Description,
            QualityRating = model.QualityRating,
            CommentTopicId = model.CommentTopicId,
            AffordableRating = model.AffordableRating
        };
    }

    public PagedResult<GetAllProductCommentResponse> Map(PagedResult<GetAllProductCommentResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllProductCommentResponse
            {
                Id = x.Id,
                UserId = x.UserId,
                ProductId = x.ProductId,
                Description = x.Description,
                CommentRate = x.CommentRate,
                QualityRating = x.QualityRating,
                CommentTopicId = x.CommentTopicId,
                UserName = x.UserName,
                UserFirstName = x.UserFirstName,
                UserLastName = x.UserLastName,
                ProductTitle = x.ProductTitle,
                AffordableRating = x.AffordableRating,
                CommentTopicTitle = x.CommentTopicTitle,
                IsActive = x.IsActive,
            })
            .ToList();

        return PagedResult<GetAllProductCommentResponse>.Create(items, model);
    }

    public PagedResult<SearchProductCommentResponse> Map(PagedResult<SearchProductCommentResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchProductCommentResponse
            {
                Id = x.Id,
                ParentId = x.ParentId,
                CreatedOnUtc = x.CreatedOnUtc,
                UserId = x.UserId,
                ProductId = x.ProductId,
                Description = x.Description,
                CommentRate = x.CommentRate,
                QualityRating = x.QualityRating,
                CommentTopicId = x.CommentTopicId,
                UserName = x.UserName,
                UserFirstName = x.UserFirstName,
                UserLastName = x.UserLastName,
                UserAvatarUrl = UserDirectory.GetImageUrl(x.UserProfileImageFileName),
                ProductTitle = x.ProductTitle,
                AffordableRating = x.AffordableRating,
                CommentTopicTitle = x.CommentTopicTitle,
                IsBuyer = x.IsBuyer,
                HelpfulCount = x.HelpfulCount,
                NotHelpfulCount = x.NotHelpfulCount,
            })
            .ToList();

        return PagedResult<SearchProductCommentResponse>.Create(items, model);
    }

    public GetAllProductCommentRequestDto Map(GetAllProductCommentRequest model)
    {
        return new GetAllProductCommentRequestDto
        {
            UserId = model.UserId,
            IsActive = model.IsActive,
            ProductId = model.ProductId,
            Pagination = model.Pagination,
            CommentTopicId = model.CommentTopicId
        }.WithContentPolicy<ProductComment, GetAllProductCommentRequestDto>(model);
    }

    public SearchProductCommentRequestDto Map(SearchProductCommentRequest model)
    {
        return new SearchProductCommentRequestDto
        {
            UserId = model.UserId,            
            ProductId = model.ProductId,
            Pagination = model.Pagination,
            CommentTopicId = model.CommentTopicId
        }.WithContentPolicy<ProductComment, SearchProductCommentRequestDto>(model);
    }
}
