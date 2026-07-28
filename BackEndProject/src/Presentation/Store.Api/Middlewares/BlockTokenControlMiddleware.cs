using System.Net;
using System.Text.Json;
using Edition.Application.Contracts;
using Edition.Application.Models.Services;
using Store.Common.Models;
using Store.Common.Localization;

namespace Store.Api.Middlewares;

/// <summary>
/// Check Is Blocked Token.
/// </summary>
/// <param name="accountingService"></param>
/// <param name="next"></param>
public class BlockTokenControlMiddleware
    (IAccountingService accountingService, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var auth = context.Request?.Headers.Authorization.ToString();
        var token = auth?.Replace("Bearer ", string.Empty, StringComparison.CurrentCultureIgnoreCase);
        if (string.IsNullOrEmpty(token))
        {
            await next(context);
            return;
        }

        var isBlocked = await accountingService.IsTokenBlockedAsync(new CheckTokenRequestDto(token));
        if (isBlocked.IsSuccess && isBlocked.Result is true)
        {
            var resourceManager = context.RequestServices.GetRequiredService<IResourceManager>();
            var error = ErrorModel.Create("InvalidToken").Localize(resourceManager);
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(error));
            return;
        }

        await next(context);
    }
}