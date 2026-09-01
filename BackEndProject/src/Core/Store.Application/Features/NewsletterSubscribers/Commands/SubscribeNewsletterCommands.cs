using Store.Common.Models;

namespace Edition.Application.Features.NewsletterSubscribers.Commands;

public record SubscribeNewsletterRequest : IRequest<OperationResult>
{
    public int LanguageId { get; init; }
    public string Email { get; init; } = default!;
}
