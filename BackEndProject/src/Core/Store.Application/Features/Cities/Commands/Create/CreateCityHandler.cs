using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Cities.Commands.Create;

public class CreateCityHandler
    (ICityRepository cityRepository, IUnitOfWork uow)
    : IRequestHandler<CreateCityRequest, OperationResult<CreateCityResponse>>
{
    public async Task<OperationResult<CreateCityResponse>> Handle(CreateCityRequest request, CancellationToken cancellationToken)
    {
        var city = City.Create(request.ProvinceId, request.Name, request.Slug, request.Description, request.Rate, request.Latitude, request.Longitude);
        cityRepository.Add(city);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateCityResponse>();

        return new CreateCityResponse { Id = city.Id };
    }
}