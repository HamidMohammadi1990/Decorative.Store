using FluentValidation;
using Edition.Application.Common.Utilities.Security;
using Edition.Application.Models.Constants;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductFiles.Commands;

public class CreateProductFileValidator : AbstractValidator<CreateProductFileRequest>
{
    private const long MaxImageSizeBytes = 5 * 1024 * 1024;

    public CreateProductFileValidator()
    {
        RuleFor(x => x.Files)
            .NotEmpty()
            .WithMessage(MessageKeys.ImageRequired);

        RuleForEach(x => x.Files).ChildRules(file =>
        {
            file.RuleFor(f => f.ProductId)
                .Must(IsValidProductId)
                .WithMessage(MessageKeys.InvalidProductId);

            file.RuleFor(f => f.Image)
                .NotNull()
                .WithMessage(MessageKeys.ImageRequired);

            file.RuleFor(f => f.Image.Length)
                .GreaterThan(0)
                .When(f => f.Image is not null)
                .WithMessage(MessageKeys.ImageRequired);

            file.RuleFor(f => f.Image.Length)
                .LessThanOrEqualTo(MaxImageSizeBytes)
                .When(f => f.Image is not null)
                .WithMessage(MessageKeys.ImageMaxSizeExceeded);
        });
    }

    private static bool IsValidProductId(string encryptedProductId)
    {
        if (string.IsNullOrWhiteSpace(encryptedProductId))
            return false;

        var decrypted = encryptedProductId.Decrypt(SecurityKeyConstant.Product);
        return int.TryParse(decrypted, out var productId) && productId > 0;
    }
}
