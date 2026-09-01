using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.NewsletterSubscribers.Queries;

public class GetAllNewsletterSubscriberHandler
    (INewsletterSubscriberRepository repository, INewsletterSubscriberMapperService mapper)
    : IRequestHandler<GetAllNewsletterSubscriberRequest, OperationResult<PagedResult<GetAllNewsletterSubscriberResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllNewsletterSubscriberResponse>>> Handle(
        GetAllNewsletterSubscriberRequest request,
        CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var result = await repository.GetAllAsync(requestModel);
        return mapper.Map(result);
    }
}
