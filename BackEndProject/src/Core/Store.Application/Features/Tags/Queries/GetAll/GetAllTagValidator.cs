using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Tags.Queries;

public class GetAllTagValidator : AbstractValidator<GetAllTagRequest>
{
    public GetAllTagValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.Tag.Title);
    }
}
