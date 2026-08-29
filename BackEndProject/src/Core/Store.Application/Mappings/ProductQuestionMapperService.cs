using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.ProductQuestions.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductQuestions;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ProductQuestionMapperService : IProductQuestionMapperService
{
    public SearchProductQuestionRequestDto Map(SearchProductQuestionRequest model)
        => new SearchProductQuestionRequestDto
        {
            ProductId = model.ProductId,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductQuestion, SearchProductQuestionRequestDto>(model);

    public GetAllProductQuestionRequestDto Map(GetAllProductQuestionRequest model)
        => new GetAllProductQuestionRequestDto
        {
            ProductId = model.ProductId,
            UserId = model.UserId,
            IsActive = model.IsActive,
            Pagination = model.Pagination
        }.WithContentPolicy<ProductQuestion, GetAllProductQuestionRequestDto>(model);

    public PagedResult<SearchProductQuestionResponse> Map(PagedResult<SearchProductQuestionResponseDto> model)
    {
        var items = model.Items
            .Select(x => new SearchProductQuestionResponse
            {
                Id = x.Id,
                ProductId = x.ProductId,
                UserId = x.UserId,
                Question = x.Question,
                Answer = x.Answer,
                CreatedOnUtc = x.CreatedOnUtc,
                UserName = x.UserName,
                UserFirstName = x.UserFirstName,
                UserLastName = x.UserLastName,
                AnsweredByFirstName = x.AnsweredByFirstName,
                AnsweredByLastName = x.AnsweredByLastName,
                AnsweredByUserName = x.AnsweredByUserName
            })
            .ToList();

        return PagedResult<SearchProductQuestionResponse>.Create(items, model);
    }

    public PagedResult<GetAllProductQuestionResponse> Map(PagedResult<GetAllProductQuestionResponseDto> model)
    {
        var items = model.Items
            .Select(x => new GetAllProductQuestionResponse
            {
                Id = x.Id,
                UserId = x.UserId,
                ProductId = x.ProductId,
                ProductTitle = x.ProductTitle,
                Question = x.Question,
                Answer = x.Answer,
                CreatedOnUtc = x.CreatedOnUtc,
                IsActive = x.IsActive,
                UserName = x.UserName,
                UserFirstName = x.UserFirstName,
                UserLastName = x.UserLastName,
                AnsweredByFirstName = x.AnsweredByFirstName,
                AnsweredByLastName = x.AnsweredByLastName,
                AnsweredByUserName = x.AnsweredByUserName
            })
            .ToList();

        return PagedResult<GetAllProductQuestionResponse>.Create(items, model);
    }
}
