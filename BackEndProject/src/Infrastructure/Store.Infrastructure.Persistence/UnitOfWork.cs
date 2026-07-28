using Microsoft.Extensions.Logging;
using Edition.Application.Contracts.Persistence;
using Store.Common.Enums;
using Store.Common.Models;

namespace Store.Infrastructure.Persistence;

public class UnitOfWork
    (EditionDbContext context, ILogger<UnitOfWork> logger, ISaveChangesExceptionReporting exceptionReporting)
    : IUnitOfWork
{
    public async Task<OperationResult> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
        catch (Exception exception)
        {
            var error = DbSaveChangesExceptionMapper.Map(exception, exceptionReporting.IncludeTechnicalDetails);

            if (DbSaveChangesExceptionMapper.IsBusinessRuleViolation(exception))
                logger.LogWarning(exception, "SaveChanges rejected by database constraint: {ErrorCode}", error.Code);
            else
                logger.LogError(exception, "SaveChanges failed: {ErrorCode}", error.Code);

            return new OperationResult(false, OperationStatusCode.ServerError, error);
        }
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}