using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Provinces.Commands;

public class UpdateProvinceHandler
    (IProvinceRepository provinceRepository, IUnitOfWork uow)
    : IRequestHandler<UpdateProvinceRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateProvinceRequest request, CancellationToken cancellationToken)
    {
        var province = await provinceRepository.FindAsync(request.Id);
        if (province is null)
            return ErrorModel.Create("InvalidId");

        province.Update(request.Name, request.Slug, request.TelPrefix, request.Description, request.Rate, request.Latitude, request.Longitude);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}