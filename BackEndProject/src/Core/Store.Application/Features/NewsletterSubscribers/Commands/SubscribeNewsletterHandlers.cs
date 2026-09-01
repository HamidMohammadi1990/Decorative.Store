using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.NewsletterSubscribers.Commands;

public class SubscribeNewsletterHandler
    (IUnitOfWork uow, INewsletterSubscriberRepository repository)
    : IRequestHandler<SubscribeNewsletterRequest, OperationResult>
{
    public async Task<OperationResult> Handle(SubscribeNewsletterRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var existing = await repository.GetByEmailAsync(email, cancellationToken);

        if (existing is not null)
        {
            if (existing.IsActive)
                return OperationResult.Success();

            existing.Resubscribe(request.LanguageId);
        }
        else
        {
            repository.Add(NewsletterSubscriber.Create(request.LanguageId, email));
        }

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        return saveChangesResult.IsSuccess
            ? OperationResult.Success()
            : saveChangesResult;
    }
}
