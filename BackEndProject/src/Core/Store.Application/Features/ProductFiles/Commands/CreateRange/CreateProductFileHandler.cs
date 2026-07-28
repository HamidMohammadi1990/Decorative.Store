using Edition.Application.Common.Directories;
using Edition.Application.Common.Utilities.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductFiles.Commands;

public class CreateProductFileHandler
     (IUnitOfWork uow, IProductFileRepository productFileRepository, ILocalFileService localFileService)
    : IRequestHandler<CreateProductFileRequest, OperationResult<List<CreateProductFileResponse>>>
{
    public async Task<OperationResult<List<CreateProductFileResponse>>> Handle(CreateProductFileRequest request, CancellationToken cancellationToken)
    {
        var productFilesTask = request.Files.Select(async file =>
        {
            var filename = await localFileService.SaveFileAsync(file.Image, ProductDirectory.ProductImage);
            if (filename.IsSuccess)
            {
                var productFile = ProductFile.Create(file.Title, file.ProductId, filename.Result!, file.IsIndex);
                productFileRepository.Add(productFile);
                return productFile;
            }
            return null;
        });

        var productFiles = await Task.WhenAll(productFilesTask);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<List<CreateProductFileResponse>>();

        var result =
             productFiles
            .Where(x => x is not null)
            .Select(x => new CreateProductFileResponse
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = ProductDirectory.GetImageUrl(x.FileName)
            }).ToList();
        return result;
    }
}