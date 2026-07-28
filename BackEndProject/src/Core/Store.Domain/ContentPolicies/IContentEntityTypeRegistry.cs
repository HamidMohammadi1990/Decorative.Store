namespace Store.Domain.ContentPolicies;

public interface IContentEntityTypeRegistry
{
    bool IsRegistered(string entityTypeName);
    Type GetClrType(string entityTypeName);
    string GetEntityPrefix(string entityTypeName);
    IReadOnlyList<string> GetRegisteredNamesOrderedByLengthDesc();
    IReadOnlyList<string> GetRegisteredEntityTypeNames();
}
