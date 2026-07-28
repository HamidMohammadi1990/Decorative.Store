namespace Store.Domain.QueryFilters;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class QueryFilterAttribute : Attribute
{
    public FilterOperator Operator { get; set; } = FilterOperator.Equal;

    /// <summary>
    /// Dot-separated path on the query root (e.g. "blogPostLike.BlogPostId").
    /// Defaults to the request property name.
    /// </summary>
    public string? MemberPath { get; set; }

    public bool IgnoreWhenNull { get; set; } = true;

    public bool IgnoreWhenEmpty { get; set; } = true;

    public bool IgnoreWhenDefault { get; set; }
}