using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyRules.Commands;

public class CreateProductPropertyRuleHandler
    (IUnitOfWork uow, IProductPropertyRuleRepository productPropertyRuleRepository)
    : IRequestHandler<CreateProductPropertyRuleRequest, OperationResult<CreateProductPropertyRuleResponse>>
{
    public async Task<OperationResult<CreateProductPropertyRuleResponse>> Handle(CreateProductPropertyRuleRequest request, CancellationToken cancellationToken)
    {
        var productPropertyRule = ProductPropertyRule.Create(
            request.IsMandatory,
            request.ProductPropertyId,
            request.PropertyType,
            request.IsActive,
            request.MinLength,
            request.MaxLength,
            request.MinQuantity,
            request.MaxQuantity,
            request.MinWidth,
            request.MaxWidth,
            request.MinHeight,
            request.MaxHeight);

        productPropertyRule.UpsertTranslation(request.LanguageId, request.Description);
        productPropertyRuleRepository.Add(productPropertyRule);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateProductPropertyRuleResponse>();

        return new CreateProductPropertyRuleResponse { Id = productPropertyRule.Id };
    }
}
