using Microsoft.AspNetCore.Http;
using Edition.Application.Models.Dtos;
using Store.Common.Models;

namespace Edition.Application.Contracts.Infrastructure;

public interface IImageService
{
    Task<OperationResult<ImageInspectorResultDto>> GetImageMetaDataAsync(IFormFile file);
}