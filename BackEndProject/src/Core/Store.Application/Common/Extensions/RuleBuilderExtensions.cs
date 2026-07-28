using FluentValidation;
using System.Text.RegularExpressions;
using Store.Common.Localization;

namespace Edition.Application.Common.Extensions;

public static class RuleBuilderExtensions
{
    private static readonly Regex IPRegex = new(
        @"^(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)\." +
        @"(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)\." +
        @"(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)\." +
        @"(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)$",
        RegexOptions.Compiled);

    public static IRuleBuilderOptions<T, string> IsValidIP<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(ip => !string.IsNullOrEmpty(ip) && IPRegex.IsMatch(ip))
            .WithMessage(MessageKeys.InvalidIpAddress);
    }
}
