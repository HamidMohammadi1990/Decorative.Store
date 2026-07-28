using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductFiles.Commands;

public class CreateProductFileValidator : AbstractValidator<CreateProductFileRequest>
{
    public CreateProductFileValidator()
    {
        RuleForEach(x => x.Files.Select(x => x.Image.Length))
            .GreaterThanOrEqualTo(50000)
            .WithMessage(MessageKeys.ImageMaxSizeExceeded)
            .Equal(0)
            .WithMessage(MessageKeys.ImageRequired);
        RuleForEach(x => x.Files.Select(x => x.ProductId))
            .Equal(0)
            .WithMessage(MessageKeys.InvalidProductId);
    }
}