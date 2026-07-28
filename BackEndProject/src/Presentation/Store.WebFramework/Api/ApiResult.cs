using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Store.Common.Models;
using Store.Common.Enums;

namespace Store.WebFramework.Api;

public class ApiResult(bool isSuccess, OperationStatusCode statusCode, params ErrorModel[] messages)
{
    public bool IsSuccess { get; set; } = isSuccess;
    public OperationStatusCode StatusCode { get; set; } = statusCode;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<OperationError> Messages { get; set; } =
        messages?.Select(x => new OperationError(x.Code, x.Message)).ToList() ?? [];

    

    public static implicit operator ApiResult(OkResult result)
    {
        return new ApiResult(true, OperationStatusCode.OK);
    }

    public static implicit operator ApiResult(BadRequestResult result)
    {
        return new ApiResult(false, OperationStatusCode.BadRequest);
    }

    public static implicit operator ApiResult(NotFoundResult result)
    {
        return new ApiResult(false, OperationStatusCode.NotFound);
    }

    public static implicit operator ApiResult(OperationResult result)
    {
        return new ApiResult(
            result.IsSuccess,
            result.Status,
            result.Messages);
    }    
}

public class ApiResult<TData>(bool isSuccess, OperationStatusCode statusCode, TData? data, params ErrorModel[] messages)
    : ApiResult(isSuccess, statusCode, messages)
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public TData? Data { get; set; } = data;


    
    public static implicit operator ApiResult<TData>(TData data)
    {
        return new ApiResult<TData>(true, OperationStatusCode.OK, data);
    }

    public static implicit operator ApiResult<TData>(OkResult result)
    {
        return new ApiResult<TData>(true, OperationStatusCode.OK, default);
    }

    public static implicit operator ApiResult<TData>(OkObjectResult result)
    {
        return new ApiResult<TData>(true, OperationStatusCode.OK, (TData)result.Value);
    }

    public static implicit operator ApiResult<TData>(BadRequestResult result)
    {
        return new ApiResult<TData>(false, OperationStatusCode.BadRequest, default);
    }

    public static implicit operator ApiResult<TData>(NotFoundResult result)
    {
        return new ApiResult<TData>(false, OperationStatusCode.NotFound, default);
    }

    public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
    {
        return new ApiResult<TData>(false, OperationStatusCode.NotFound, (TData)result.Value);
    }

    public static implicit operator ApiResult<TData>(OperationResult<TData> result)
    {
        return new ApiResult<TData>(result.IsSuccess, result.Status, result.Result, result.Messages);
    }    
}