using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.AssistantFaqs.Queries;

public class GetAllAssistantFaqHandler
    (IAssistantFaqRepository repository, IAssistantFaqMapperService mapper)
    : IRequestHandler<GetAllAssistantFaqRequest, OperationResult<PagedResult<GetAllAssistantFaqResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllAssistantFaqResponse>>> Handle(
        GetAllAssistantFaqRequest request,
        CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var result = await repository.GetAllAsync(requestModel);
        return mapper.Map(result);
    }
}

public class GetAssistantFaqHandler
    (IAssistantFaqRepository repository, IAssistantFaqMapperService mapper)
    : IRequestHandler<GetAssistantFaqRequest, OperationResult<GetAssistantFaqResponse?>>
{
    public async Task<OperationResult<GetAssistantFaqResponse?>> Handle(
        GetAssistantFaqRequest request,
        CancellationToken cancellationToken)
    {
        var entity = await repository.GetAsNoTrackingAsync(request.Id, cancellationToken);
        if (entity is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(entity);
    }
}

public class SearchAssistantFaqHandler
    (IAssistantFaqRepository repository, IAssistantFaqMapperService mapper)
    : IRequestHandler<SearchAssistantFaqRequest, OperationResult<PagedResult<SearchAssistantFaqResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchAssistantFaqResponse>>> Handle(
        SearchAssistantFaqRequest request,
        CancellationToken cancellationToken)
    {
        if (request.LanguageId <= 0)
            return ErrorModel.Create("InvalidRequest");

        var requestModel = mapper.Map(request);
        var result = await repository.SearchAsync(requestModel);
        return mapper.MapSearch(result);
    }
}
