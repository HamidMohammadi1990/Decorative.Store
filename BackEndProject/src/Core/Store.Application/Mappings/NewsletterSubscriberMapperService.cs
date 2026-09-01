using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.NewsletterSubscribers.Queries;
using Store.Domain.Dtos.NewsletterSubscribers;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Mappings;

public class NewsletterSubscriberMapperService : INewsletterSubscriberMapperService
{
    public GetAllNewsletterSubscriberRequestDto Map(GetAllNewsletterSubscriberRequest model)
        => new()
        {
            LanguageId = model.LanguageId,
            Email = model.Email,
            Pagination = model.Pagination,
        };

    public PagedResult<GetAllNewsletterSubscriberResponse> Map(PagedResult<GetAllNewsletterSubscriberResponseDto> model)
    {
        var items = model.Items
            .Select(x => new GetAllNewsletterSubscriberResponse
            {
                Id = x.Id,
                Email = x.Email,
                LanguageId = x.LanguageId,
                SubscribedAtUtc = x.SubscribedAtUtc,
                IsActive = x.IsActive,
            })
            .ToList();

        return PagedResult<GetAllNewsletterSubscriberResponse>.Create(items, model);
    }
}
