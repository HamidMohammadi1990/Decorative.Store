using System.Net;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Store.WebFramework.Api;
using Store.Common.Models;
using Store.Common.Enums;
using Store.Common.Exceptions;
using Store.Common.Localization;
using Store.Common.Extensions;

namespace Store.Api.Middlewares;

public class CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            context.Request.Headers["X-Correlation-ID"] = correlationId;
        }

        AddCorrelationIdOnStarting();

        int userId = context.User.Identity!.GetUserId<int>();

        using var correlationScope = Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId);
        using var userScope = Serilog.Context.LogContext.PushProperty("UserId", userId);

        try
        {
            await next(context);
        }
        catch (AppValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (AppException ex)
        {
            await HandleAppException(context, ex);
        }
        catch (SecurityTokenExpiredException ex)
        {
            await HandleUnauthorizeExceptionAsync(context, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleUnauthorizeExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleUnexpectedExceptionAsync(context, ex);
        }


        void AddCorrelationIdOnStarting()
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers["X-Correlation-ID"] = correlationId.ToString();
                return Task.CompletedTask;
            });
        }
    }

    private async Task HandleAppException(HttpContext context, AppException ex)
    {
        var resourceManager = context.RequestServices.GetRequiredService<IResourceManager>();
        logger.LogError(ex, ex.Message);
        await WriteResponseAsync(
            context,
            ex.HttpStatusCode,
            ApiResultLocalizer.Localize(
                new ApiResult(false, ex.ApiStatusCode, ErrorModel.CreateLiteral("GeneralException", ex.Message)),
                resourceManager)
        );
    }

    private static async Task HandleValidationExceptionAsync(HttpContext context, AppValidationException ex)
    {
        var resourceManager = context.RequestServices.GetRequiredService<IResourceManager>();
        var result = ApiResultLocalizer.Localize(
            new ApiResult(
                false,
                OperationStatusCode.OK,
                [.. ex.Errors.Select(x => ErrorModel.CreateLiteral("ValidationError", x))]),
            resourceManager);

        await WriteResponseAsync(context, HttpStatusCode.OK, result);
    }

    private async Task HandleUnauthorizeExceptionAsync(HttpContext context, Exception ex)
    {
        var resourceManager = context.RequestServices.GetRequiredService<IResourceManager>();
        logger.LogError(ex, ex.Message);

        var result = ApiResultLocalizer.Localize(
            new ApiResult(
                false,
                OperationStatusCode.UnAuthorized,
                ErrorModel.Create("AuthException")),
            resourceManager);

        await WriteResponseAsync(context, HttpStatusCode.Unauthorized, result);
    }

    private async Task HandleUnexpectedExceptionAsync(HttpContext context, Exception ex)
    {
        var resourceManager = context.RequestServices.GetRequiredService<IResourceManager>();
        logger.LogError(ex, ex.Message);

#if DEBUG
        var detail = JsonSerializer.Serialize(new
        {
            Exception = ex.Message,
            ex.StackTrace
        });
#else
        var detail = resourceManager.GetString("UnhandledException");
#endif

        var result = ApiResultLocalizer.Localize(
            new ApiResult(
                false,
                OperationStatusCode.ServerError,
                ErrorModel.CreateLiteral("UnhandledException", detail)),
            resourceManager);

        await WriteResponseAsync(context, HttpStatusCode.InternalServerError, result);
    }

    private static async Task WriteResponseAsync(
        HttpContext context,
        HttpStatusCode code,
        ApiResult result)
    {
        if (context.Response.HasStarted)
            throw new InvalidOperationException("Response already started.");

        var json = JsonSerializer.Serialize(result);

        context.Response.StatusCode = (int)code;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(json);
    }
}