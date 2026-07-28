namespace Store.Domain.Common;

public abstract class BaseEntity<TKey> : IEntity<TKey> where TKey : notnull
{
    public TKey Id { get; protected set; } = default!;
}

public abstract class BaseEntity : BaseEntity<int>, IEntity;
