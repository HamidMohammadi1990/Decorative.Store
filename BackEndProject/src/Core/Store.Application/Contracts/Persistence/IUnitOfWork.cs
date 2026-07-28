using Store.Common.Models;

namespace Edition.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{    
    /// <summary>
    /// Save all entities in to database.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationResult> SaveChangesAsync(CancellationToken cancellationToken = default);
}