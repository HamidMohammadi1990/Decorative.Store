using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Banks.Queries;

public class GetBankHandler
    (IBankRepository bankRepository, IBankMapperService mapper)
    : IRequestHandler<GetBankRequest, OperationResult<GetBankResponse?>>
{
    public async Task<OperationResult<GetBankResponse?>> Handle(GetBankRequest request, CancellationToken cancellationToken)
    {
        var bank = await bankRepository.GetAsNoTrackingAsync(request.Id);
        if (bank is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(bank);
        return result;
    }
}
