using Edition.Application.Features.NewsletterSubscribers.Queries;
using Store.Domain.Dtos.NewsletterSubscribers;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Contracts.Mapping;

public interface INewsletterSubscriberMapperService : IMapper
{
    GetAllNewsletterSubscriberRequestDto Map(GetAllNewsletterSubscriberRequest model);
    PagedResult<GetAllNewsletterSubscriberResponse> Map(PagedResult<GetAllNewsletterSubscriberResponseDto> model);
}
