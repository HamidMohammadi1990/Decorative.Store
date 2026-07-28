using System.Linq.Expressions;
using Store.Domain.Enums;

namespace Edition.Application.Contracts.ContentPolicies;

public interface IContentPolicyFilteredRequest
{
    Type EntityClrType { get; }

    /// <summary>Optional override for standard actions.</summary>
    ContentPolicyQueryAction? ContentPolicyQueryAction { get; }

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    LambdaExpression? ContentPolicyFilter { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    bool IsContentPolicyResolved { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    bool ContentPolicyDenyAll { get; set; }
}

public interface IContentPolicyFilteredRequest<TEntity> : IContentPolicyFilteredRequest
    where TEntity : class
{
    Type IContentPolicyFilteredRequest.EntityClrType => typeof(TEntity);

    ContentPolicyQueryAction? IContentPolicyFilteredRequest.ContentPolicyQueryAction => null;
}
