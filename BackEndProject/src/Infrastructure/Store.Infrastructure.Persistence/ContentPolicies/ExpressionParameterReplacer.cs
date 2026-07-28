using System.Linq.Expressions;

namespace Store.Infrastructure.Persistence.ContentPolicies;

internal sealed class ExpressionParameterReplacer(ParameterExpression source, ParameterExpression target)
    : ExpressionVisitor
{
    protected override Expression VisitParameter(ParameterExpression node)
        => node == source ? target : base.VisitParameter(node);
}