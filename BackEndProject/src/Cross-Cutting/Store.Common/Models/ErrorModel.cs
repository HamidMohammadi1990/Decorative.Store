namespace Store.Common.Models;

public record ErrorModel
{
    public string Code { get; init; } = default!;
    public string Message { get; init; } = string.Empty;
    public object[]? FormatArgs { get; init; }
    public bool UseResourceMessage { get; init; } = true;

    public static ErrorModel Create(string code)
        => new() { Code = code, UseResourceMessage = true };

    public static ErrorModel Create(string code, params object[] formatArgs)
        => new() { Code = code, FormatArgs = formatArgs, UseResourceMessage = true };

    public static ErrorModel CreateLiteral(string code, string message)
        => new() { Code = code, Message = message, UseResourceMessage = false };
}