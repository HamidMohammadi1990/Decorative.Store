using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductFiles.Queries;

public class SearchProductFileValidator : AbstractValidator<SearchProductFileRequest>
{
    public SearchProductFileValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.ProductFile.Title);
    }
}
