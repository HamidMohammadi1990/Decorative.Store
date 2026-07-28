using Store.Common.Localization;
using Store.Common.Models;

namespace Store.WebFramework.Api;

public static class ApiResultLocalizer
{
    public static ApiResult Localize(ApiResult result, IResourceManager resourceManager)
    {
        result.Messages = result.Messages.Select(message => LocalizeMessage(message, resourceManager)).ToList();
        return result;
    }

    public static ErrorModel[] LocalizeErrors(IEnumerable<ErrorModel> errors, IResourceManager resourceManager)
        => errors.Select(error => error.Localize(resourceManager)).ToArray();

    private static OperationError LocalizeMessage(OperationError message, IResourceManager resourceManager)
    {
        if (!string.IsNullOrWhiteSpace(message.Message))
            return new OperationError(message.Code, resourceManager.ResolveMessage(message.Message));

        var localized = ErrorModel.Create(message.Code).Localize(resourceManager);
        return new OperationError(message.Code, localized.Message);
    }
}
