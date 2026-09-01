using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.AssistantFaqs.Commands;

public class CreateAssistantFaqHandler
    (IUnitOfWork uow, IAssistantFaqRepository repository)
    : IRequestHandler<CreateAssistantFaqRequest, OperationResult<CreateAssistantFaqResponse>>
{
    public async Task<OperationResult<CreateAssistantFaqResponse>> Handle(
        CreateAssistantFaqRequest request,
        CancellationToken cancellationToken)
    {
        var entity = AssistantFaq.Create(
            request.LanguageId,
            request.Question.Trim(),
            request.Answer.Trim(),
            request.Priority);

        repository.Add(entity);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateAssistantFaqResponse>();

        return new CreateAssistantFaqResponse { Id = entity.Id };
    }
}

public class UpdateAssistantFaqHandler
    (IUnitOfWork uow, IAssistantFaqRepository repository)
    : IRequestHandler<UpdateAssistantFaqRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateAssistantFaqRequest request, CancellationToken cancellationToken)
    {
        var entity = await repository.FindAsync(request.Id, cancellationToken);
        if (entity is null)
            return ErrorModel.Create("InvalidId");

        entity.Update(
            request.LanguageId,
            request.Question.Trim(),
            request.Answer.Trim(),
            request.Priority,
            request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}

public class DeleteAssistantFaqHandler
    (IUnitOfWork uow, IAssistantFaqRepository repository)
    : IRequestHandler<DeleteAssistantFaqRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteAssistantFaqRequest request, CancellationToken cancellationToken)
    {
        var entity = await repository.FindAsync(request.Id, cancellationToken);
        if (entity is null)
            return ErrorModel.Create("InvalidId");

        repository.Remove(entity);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
