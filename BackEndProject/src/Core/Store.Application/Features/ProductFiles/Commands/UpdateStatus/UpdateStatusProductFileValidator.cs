using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductFiles.Commands;

public class UpdateStatusProductFileValidator : AbstractValidator<UpdateStatusProductFileRequest>
{
    public UpdateStatusProductFileValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidId);
    }
}