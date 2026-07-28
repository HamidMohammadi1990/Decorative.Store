using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Provinces.Commands;

public class CreateProvinceHandler
    (IProvinceRepository provinceRepository, IUnitOfWork uow)
    : IRequestHandler<CreateProvinceRequest, OperationResult<CreateProvinceResponse>>
{
    public async Task<OperationResult<CreateProvinceResponse>> Handle(CreateProvinceRequest request, CancellationToken cancellationToken)
    {
        var province = Province.Create(request.Name, request.Slug, request.TelPrefix, request.Description, request.Rate, request.Latitude, request.Longitude);
        provinceRepository.Add(province);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateProvinceResponse>();

        return new CreateProvinceResponse { Id = province.Id };
    }
}