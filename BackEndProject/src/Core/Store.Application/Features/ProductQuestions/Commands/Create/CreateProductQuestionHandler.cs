using Store.Common.Extensions;
using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductQuestions.Commands;

public class CreateProductQuestionHandler
    (IUnitOfWork uow, IProductQuestionRepository productQuestionRepository, ICurrentUserContext currentUser)
    : IRequestHandler<CreateProductQuestionRequest, OperationResult<CreateProductQuestionResponse>>
{
    public async Task<OperationResult<CreateProductQuestionResponse>> Handle(
        CreateProductQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var productQuestion = ProductQuestion.Create(
            currentUser.UserId,
            request.ProductId,
            request.Question);

        productQuestionRepository.Add(productQuestion);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateProductQuestionResponse>();

        return new CreateProductQuestionResponse { Id = productQuestion.Id };
    }
}
