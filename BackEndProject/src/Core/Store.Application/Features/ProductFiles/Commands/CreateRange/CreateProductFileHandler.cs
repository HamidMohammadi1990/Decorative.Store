using Edition.Application.Common.Directories;
using Edition.Application.Common.Utilities.Contracts;
using Edition.Application.Common.Utilities.Security;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Models.Constants;
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
            var productId = ResolveProductId(file.ProductId);
            if (productId == 0)
                return (productFile: (ProductFile?)null, title: (string?)null);

            var filename = await localFileService.SaveFileAsync(file.Image, ProductDirectory.ProductImage);
            if (filename.IsSuccess)
            {
                var productFile = ProductFile.Create(productId, filename.Result!, file.IsIndex);
                productFile.UpsertTranslation(file.LanguageId, file.Title);
                productFileRepository.Add(productFile);
                return (productFile: productFile, title: file.Title);
            }
            return (productFile: (ProductFile?)null, title: (string?)null);
        });

        var productFiles = await Task.WhenAll(productFilesTask);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<List<CreateProductFileResponse>>();

        var result =
             productFiles
            .Where(x => x.productFile is not null)
            .Select(x => new CreateProductFileResponse
            {
                Id = x.productFile!.Id,
                Title = x.title!,
                ImageUrl = ProductDirectory.GetImageUrl(x.productFile.FileName)
            }).ToList();
        return result;
    }

    private static int ResolveProductId(string encryptedProductId)
    {
        if (string.IsNullOrWhiteSpace(encryptedProductId))
            return 0;

        var decrypted = encryptedProductId.Decrypt(SecurityKeyConstant.Product);
        return int.TryParse(decrypted, out var productId) ? productId : 0;
    }
}
