namespace Store.Domain.Enums;

public enum ContentPolicyAccessMode
{
    /// <summary>No filter applied — full access (bypass or no policy requirement).</summary>
    Unrestricted = 0,

    /// <summary>Filter denies every row.</summary>
    DenyAll = 1,

    /// <summary>A compiled allow/deny filter is applied.</summary>
    Filtered = 2
}
