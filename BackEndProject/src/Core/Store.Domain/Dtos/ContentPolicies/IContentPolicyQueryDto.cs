using System.Linq.Expressions;

namespace Store.Domain.Dtos.ContentPolicies;

public interface IContentPolicyQueryDto<TEntity>
    where TEntity : class
{
    Expression<Func<TEntity, bool>>? ContentFilter { get; set; }
}
