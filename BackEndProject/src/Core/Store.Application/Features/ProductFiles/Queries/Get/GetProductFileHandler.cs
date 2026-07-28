using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductFiles.Queries;

public class GetProductFileHandler
     (IProductFileRepository productFileRepository, IProductFileMapperService mapper)
    : IRequestHandler<GetProductFileRequest, OperationResult<GetProductFileResponse?>>
{
    public async Task<OperationResult<GetProductFileResponse?>> Handle(GetProductFileRequest request, CancellationToken cancellationToken)
    {
        var productFile = await productFileRepository.GetAsNoTrackingAsync(request.Id);
        if (productFile is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(productFile);
        return result;
    }
}