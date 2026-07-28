using Microsoft.AspNetCore.Http;
using Edition.Application.Common.Utilities;
using Edition.Application.Common.Utilities.Contracts;
using Store.Common.Models;

namespace Edition.Application.Common.Utilities.Services;

public class LocalFileService : ILocalFileService
{
    public OperationResult DeleteDirectory(string directoryPath)
    {
        try
        {
            if (Directory.Exists(directoryPath))
                Directory.Delete(directoryPath, true);

            return OperationResult.Success();
        }
        catch
        {
            return OperationResult.Fail();
        }
    }

    public OperationResult DeleteFile(string path, string fileName)
    {
        try
        {
            var rootDirectory = Path.Combine(Directory.GetCurrentDirectory(), path);
            var filePath = FileUploadSecurity.ResolveSafeFilePath(rootDirectory, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            return OperationResult.Success();
        }
        catch
        {
            return OperationResult.Fail();
        }
    }

    public OperationResult DeleteFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            return OperationResult.Success();
        }
        catch
        {
            return OperationResult.Fail();
        }
    }

    public async Task<OperationResult> SaveFile(IFormFile file, string directoryPath)
    {
        try
        {
            if (file is null)
                return OperationResult.Fail($"{nameof(file)} is required");

            var fileName = FileUploadSecurity.CreateSafeFileName(file.FileName);
            var folderName = Path.Combine(Directory.GetCurrentDirectory(), directoryPath.Replace("/", "\\"));

            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            var path = FileUploadSecurity.ResolveSafeFilePath(folderName, fileName);
            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return OperationResult.Success();
        }
        catch
        {
            return OperationResult.Fail();
        }
    }

    public async Task<OperationResult> SaveFileAsync(IFormFile file, string directoryPath, string fileName)
    {
        try
        {
            if (file is null)
                return OperationResult.Fail($"{nameof(file)} is required");

            var folderName = Path.Combine(Directory.GetCurrentDirectory(), directoryPath.Replace("/", "\\"));
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            var path = FileUploadSecurity.ResolveSafeFilePath(folderName, fileName);
            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return OperationResult.Success();
        }
        catch
        {
            return OperationResult.Fail();
        }
    }

    public async Task<OperationResult<string>> SaveFileAsync(IFormFile file, string directoryPath)
    {
        try
        {
            if (file is null)
                return OperationResult<string>.Fail($"{nameof(file)} is required");

            var fileName = FileUploadSecurity.CreateSafeFileName(file.FileName);
            var folderName = Path.Combine(Directory.GetCurrentDirectory(), directoryPath.Replace("/", "\\"));

            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            var path = FileUploadSecurity.ResolveSafeFilePath(folderName, fileName);
            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return fileName;
        }
        catch
        {
            return OperationResult<string>.Fail();
        }
    }
}
