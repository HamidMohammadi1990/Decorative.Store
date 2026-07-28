using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence;
using Store.Domain.Common;

namespace Store.Infrastructure.Persistence.Repositories;

public abstract class Repository<TEntity, TKey>(EditionDbContext context)
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{
    protected EditionDbContext Context { get; } = context;

    private DbSet<TEntity> DbSet { get; } = context.Set<TEntity>();

    public virtual ValueTask<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken = default)
        => DbSet.FindAsync([id!], cancellationToken);

    public virtual Task<TEntity?> GetAsNoTrackingAsync(TKey id, CancellationToken cancellationToken = default)
        => DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

    public void Add(TEntity entity)
        => DbSet.Add(entity);

    public void Remove(TEntity entity)
        => DbSet.Remove(entity);

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        => DbSet.AnyAsync(expression, cancellationToken);
}

public abstract class Repository<TEntity>(EditionDbContext context)
    : Repository<TEntity, int>(context)
    where TEntity : BaseEntity;