using Store.Common.Models;

namespace Store.Common.Localization;

public static class ErrorLocalization
{
    public static ErrorModel Localize(this ErrorModel error, IResourceManager resourceManager)
    {
        if (!error.UseResourceMessage)
            return error;

        var message = error.FormatArgs is { Length: > 0 }
            ? resourceManager.GetString(error.Code, error.FormatArgs)
            : resourceManager.GetString(error.Code);

        return error with { Message = message };
    }

    public static IEnumerable<ErrorModel> Localize(this IEnumerable<ErrorModel> errors, IResourceManager resourceManager)
        => errors.Select(error => error.Localize(resourceManager));
}
