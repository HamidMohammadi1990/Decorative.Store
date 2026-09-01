using FluentValidation;
using Edition.Application.Common.Utilities.Security;
using Edition.Application.Common.Validation;
using Edition.Application.Models.Constants;
using Store.Common.Localization;

namespace Edition.Application.Features.BlogPostFiles.Commands;

public class CreateBlogPostFileValidator : AbstractValidator<CreateBlogPostFileRequest>
{
    private const long MaxImageSizeBytes = 5 * 1024 * 1024;

    public CreateBlogPostFileValidator()
    {
        RuleFor(x => x.Files)
            .NotEmpty()
            .WithMessage(MessageKeys.ImageRequired);

        RuleForEach(x => x.Files).ChildRules(file =>
        {
            file.RuleFor(f => f.BlogPostId)
                .Must(IsValidBlogPostId)
                .WithMessage(MessageKeys.InvalidId);

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

    private static bool IsValidBlogPostId(string encryptedBlogPostId)
    {
        if (string.IsNullOrWhiteSpace(encryptedBlogPostId))
            return false;

        var decrypted = encryptedBlogPostId.Decrypt(SecurityKeyConstant.BlogPost);
        return int.TryParse(decrypted, out var blogPostId) && blogPostId > 0;
    }
}

public class DeleteBlogPostFileValidator : AbstractValidator<DeleteBlogPostFileRequest>
{
    public DeleteBlogPostFileValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}

public class UpdateStatusBlogPostFileValidator : AbstractValidator<UpdateStatusBlogPostFileRequest>
{
    public UpdateStatusBlogPostFileValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}

public class UpdateSetMainBlogPostFileValidator : AbstractValidator<UpdateSetMainBlogPostFileRequest>
{
    public UpdateSetMainBlogPostFileValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
