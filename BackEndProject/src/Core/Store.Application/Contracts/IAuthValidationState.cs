namespace Edition.Application.Contracts;

public interface IAuthValidationState
{
    bool? CachedResult { get; set; }
}
