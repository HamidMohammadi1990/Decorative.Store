using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductFiles.Queries;

public class GetProductFileValidator : AbstractValidator<GetProductFileRequest>
{
    public GetProductFileValidator()
    {
        RuleFor(u => u.Id)
         .NotEqual(0)
         .WithMessage(MessageKeys.InvalidId);
    }
}