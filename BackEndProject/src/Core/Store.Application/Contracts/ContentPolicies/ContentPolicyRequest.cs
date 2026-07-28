using System.Linq.Expressions;

namespace Edition.Application.Contracts.ContentPolicies;

public abstract record ContentPolicyRequest<TEntity> : IContentPolicyFilteredRequest<TEntity>
    where TEntity : class
{
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public LambdaExpression? ContentPolicyFilter { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public bool IsContentPolicyResolved { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public bool ContentPolicyDenyAll { get; set; }
}
