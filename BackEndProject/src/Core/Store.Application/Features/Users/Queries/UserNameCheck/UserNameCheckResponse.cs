namespace Edition.Application.Features.Users.Queries;

public record UserNameCheckResponse
{
    public bool HasAccount { get; init; }
    public bool IsPhoneNumber { get; init; }
}