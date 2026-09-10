using Store.Common.Models;

namespace Edition.Application.Features.Pages.Queries;

public record GetPageTypeGuidesRequest : IRequest<OperationResult<List<GetPageTypeGuidesResponse>>>;
