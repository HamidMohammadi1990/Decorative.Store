using Edition.Application.Common.Directories;
using Edition.Application.Common.Utilities.Contracts;
using Edition.Application.Common.Utilities.Security;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Models.Constants;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPostFiles.Commands;

public class CreateBlogPostFileHandler
    (IUnitOfWork uow, IBlogPostFileRepository blogPostFileRepository, ILocalFileService localFileService)
    : IRequestHandler<CreateBlogPostFileRequest, OperationResult<List<CreateBlogPostFileResponse>>>
{
    public async Task<OperationResult<List<CreateBlogPostFileResponse>>> Handle(
        CreateBlogPostFileRequest request,
        CancellationToken cancellationToken)
    {
        var blogPostIdsWithNewMain = request.Files
            .Where(file => file.IsIndex)
            .Select(file => ResolveBlogPostId(file.BlogPostId))
            .Where(blogPostId => blogPostId > 0)
            .Distinct()
            .ToList();

        foreach (var blogPostId in blogPostIdsWithNewMain)
            await blogPostFileRepository.ClearMainFlagsAsync(blogPostId, cancellationToken);

        var blogPostFiles = new List<(BlogPostFile? blogPostFile, string? title)>();

        foreach (var file in request.Files)
        {
            var blogPostId = ResolveBlogPostId(file.BlogPostId);
            if (blogPostId == 0)
            {
                blogPostFiles.Add((null, null));
                continue;
            }

            var filename = await localFileService.SaveFileAsync(file.Image, BlogPostDirectory.BlogPostImage);
            if (filename.IsSuccess)
            {
                var blogPostFile = BlogPostFile.Create(blogPostId, filename.Result!, file.IsIndex);
                blogPostFile.UpsertTranslation(file.LanguageId, file.Title);
                blogPostFileRepository.Add(blogPostFile);
                blogPostFiles.Add((blogPostFile, file.Title));
                continue;
            }

            blogPostFiles.Add((null, null));
        }

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<List<CreateBlogPostFileResponse>>();

        var result = blogPostFiles
            .Where(x => x.blogPostFile is not null)
            .Select(x => new CreateBlogPostFileResponse
            {
                Id = x.blogPostFile!.Id,
                Title = x.title!,
                ImageUrl = BlogPostDirectory.GetImageUrl(x.blogPostFile.FileName),
            })
            .ToList();

        return result;
    }

    private static int ResolveBlogPostId(string encryptedBlogPostId)
    {
        if (string.IsNullOrWhiteSpace(encryptedBlogPostId))
            return 0;

        var decrypted = encryptedBlogPostId.Decrypt(SecurityKeyConstant.BlogPost);
        return int.TryParse(decrypted, out var blogPostId) ? blogPostId : 0;
    }
}

public class DeleteBlogPostFileHandler
    (IUnitOfWork uow, IBlogPostFileRepository blogPostFileRepository, ILocalFileService localFileService)
    : IRequestHandler<DeleteBlogPostFileRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteBlogPostFileRequest request, CancellationToken cancellationToken)
    {
        var blogPostFile = await blogPostFileRepository.GetWithTranslationsAsync(request.Id, cancellationToken);
        if (blogPostFile is null)
            return ErrorModel.Create("InvalidId");

        blogPostFileRepository.Remove(blogPostFile);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        localFileService.DeleteFile(BlogPostDirectory.BlogPostImage, blogPostFile.FileName);
        return OperationResult.Success();
    }
}

public class UpdateStatusBlogPostFileHandler
    (IUnitOfWork uow, IBlogPostFileRepository blogPostFileRepository)
    : IRequestHandler<UpdateStatusBlogPostFileRequest, OperationResult>
{
    public async Task<OperationResult> Handle(
        UpdateStatusBlogPostFileRequest request,
        CancellationToken cancellationToken)
    {
        var blogPostFile = await blogPostFileRepository.FindAsync(request.Id, cancellationToken);
        if (blogPostFile is null)
            return ErrorModel.Create("InvalidId");

        blogPostFile.UpdateStatus(request.Status);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}

public class UpdateSetMainBlogPostFileHandler
    (IUnitOfWork uow, IBlogPostFileRepository blogPostFileRepository)
    : IRequestHandler<UpdateSetMainBlogPostFileRequest, OperationResult>
{
    public async Task<OperationResult> Handle(
        UpdateSetMainBlogPostFileRequest request,
        CancellationToken cancellationToken)
    {
        var blogPostFile = await blogPostFileRepository.FindAsync(request.Id, cancellationToken);
        if (blogPostFile is null)
            return ErrorModel.Create("InvalidId");

        await blogPostFileRepository.ClearMainFlagsAsync(blogPostFile.BlogPostId, cancellationToken);
        blogPostFile.SetMain(true);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
