using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Banks.Commands;

public class UpdateBankHandler
    (IBankRepository bankRepository, IUnitOfWork uow, IBankMapperService mapper)
    : IRequestHandler<UpdateBankRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateBankRequest request, CancellationToken cancellationToken)
    {
        var bank = await bankRepository.FindAsync(request.Id);
        if (bank is null)
            return ErrorModel.Create("InvalidId");

        bank.Title = request.Title;
        bank.Icon = request.Icon;
        bank.IsActive = request.IsActive;

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
