namespace Store.Domain.Common;

public interface IEntity<TKey> where TKey : notnull
{
    TKey Id { get; }
}

public interface IEntity : IEntity<int>;
