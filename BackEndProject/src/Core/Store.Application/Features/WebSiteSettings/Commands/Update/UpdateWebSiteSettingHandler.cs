using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.WebSiteSettings.Commands;

public class UpdateWebSiteSettingHandler
    (IWebSiteSettingRepository repository, IUnitOfWork uow)
    : IRequestHandler<UpdateWebSiteSettingRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateWebSiteSettingRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.GetAsync();
        if (model is null || model.Id != request.Id)
            return ErrorModel.Create("InvalidId");

        model.Update(
            request.Email,
            request.PhoneNumber,
            request.Telephone,
            request.Address,
            request.CartNumber,
            request.EmailUserName,
            request.EmailPassword,
            request.SmsAccountUrl,
            request.SmsAccountUserName,
            request.SmsAccountPassword);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
