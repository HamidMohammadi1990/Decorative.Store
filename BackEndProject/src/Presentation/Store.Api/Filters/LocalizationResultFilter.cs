using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.WebFramework.Api;
using Store.Common.Models;
using Store.Common.Localization;

namespace Store.Api.Filters;

public sealed class LocalizationResultFilter(IResourceManager resourceManager) : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult { Value: not null } objectResult)
            LocalizeValue(objectResult.Value);

        await next();
    }

    private void LocalizeValue(object value)
    {
        switch (value)
        {
            case ApiResult apiResult:
                apiResult.Messages = LocalizeMessages(apiResult.Messages);
                break;
            default:
                LocalizeGenericApiResult(value);
                break;
        }
    }

    private void LocalizeGenericApiResult(object value)
    {
        var messagesProperty = value.GetType().GetProperty(nameof(ApiResult.Messages));
        if (messagesProperty?.GetValue(value) is not List<OperationError> messages)
            return;

        messagesProperty.SetValue(value, LocalizeMessages(messages));
    }

    private List<OperationError> LocalizeMessages(List<OperationError> messages)
        => messages.Select(LocalizeMessage).ToList();

    private OperationError LocalizeMessage(OperationError message)
    {
        if (!string.IsNullOrWhiteSpace(message.Message))
            return new OperationError(message.Code, resourceManager.ResolveMessage(message.Message));

        var localized = ErrorModel.Create(message.Code).Localize(resourceManager);
        return new OperationError(message.Code, localized.Message);
    }
}