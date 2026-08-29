using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductQuestions.Commands;

public class ChangeStatusProductQuestionHandler
    (IUnitOfWork uow, IProductQuestionRepository productQuestionRepository)
    : IRequestHandler<ChangeStatusProductQuestionRequest, OperationResult>
{
    public async Task<OperationResult> Handle(
        ChangeStatusProductQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var productQuestion = await productQuestionRepository.FindAsync(request.Id, cancellationToken);
        if (productQuestion is null)
            return ErrorModel.Create("InvalidId");

        if (productQuestion.IsActive == request.IsActive)
            return OperationResult.Success();

        if (request.IsActive)
            productQuestion.Active();
        else
            productQuestion.InActive();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
