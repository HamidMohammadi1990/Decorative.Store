using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductFiles.Commands;

public class DeleteProductFileValidator : AbstractValidator<DeleteProductFileRequest>
{
    public DeleteProductFileValidator()
    {
        RuleFor(u => u.Id)
             .NotEqual(0)
             .WithMessage(MessageKeys.InvalidId);
    }
}