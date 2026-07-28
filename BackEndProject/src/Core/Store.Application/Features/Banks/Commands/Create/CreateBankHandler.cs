using Edition.Domain.Entities;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Banks.Commands;

public class CreateBankHandler
    (IUnitOfWork uow, IBankRepository bankRepository, IBankMapperService mapper)
    : IRequestHandler<CreateBankRequest, OperationResult<CreateBankResponse>>
{
    public async Task<OperationResult<CreateBankResponse>> Handle(CreateBankRequest request, CancellationToken cancellationToken)
    {
        var bank = new Bank
        {
            Icon = request.Icon,
            Title = request.Title,
            IsActive = request.IsActive
        };

        bankRepository.Add(bank);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateBankResponse>();

        return new CreateBankResponse { Id = bank.Id };
    }
}
