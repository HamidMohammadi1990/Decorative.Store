using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductQuestions.Commands;

public class AnswerProductQuestionHandler
    (IUnitOfWork uow, IProductQuestionRepository productQuestionRepository, ICurrentUserContext currentUser)
    : IRequestHandler<AnswerProductQuestionRequest, OperationResult>
{
    public async Task<OperationResult> Handle(
        AnswerProductQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var productQuestion = await productQuestionRepository.FindAsync(request.Id, cancellationToken);
        if (productQuestion is null)
            return ErrorModel.Create("InvalidId");

        var answer = request.Answer.Trim();
        if (string.IsNullOrWhiteSpace(answer))
            return ErrorModel.Create("AnswerRequired");

        productQuestion.SetAnswer(answer, currentUser.UserId);
        productQuestion.Active();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
