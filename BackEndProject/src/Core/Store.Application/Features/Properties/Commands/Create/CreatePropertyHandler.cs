using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Properties.Commands;

public class CreatePropertyHandler
    (IUnitOfWork uow, IPropertyRepository propertyRepository)
    : IRequestHandler<CreatePropertyRequest, OperationResult<CreatePropertyResponse>>
{
    public async Task<OperationResult<CreatePropertyResponse>> Handle(CreatePropertyRequest request, CancellationToken cancellationToken)
    {
        var property = Property.Create(request.PropertyType, request.ParentId, request.Title, request.PropertyCategoryId, request.Priority, request.Description);
        propertyRepository.Add(property);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreatePropertyResponse>();

        return new CreatePropertyResponse { Id = property.Id };
    }
}