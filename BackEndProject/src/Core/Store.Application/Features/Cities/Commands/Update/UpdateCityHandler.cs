using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Cities.Commands;

public class UpdateCityHandler
    (ICityRepository cityRepository, IUnitOfWork uow)
    : IRequestHandler<UpdateCityRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateCityRequest request, CancellationToken cancellationToken)
    {
        var city = await cityRepository.FindAsync(request.Id);
        if (city is null)
            return ErrorModel.Create("InvalidId");

        city.Update(request.ProvinceId, request.Name, request.Slug, request.Description, request.Rate, request.Latitude, request.Longitude);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}