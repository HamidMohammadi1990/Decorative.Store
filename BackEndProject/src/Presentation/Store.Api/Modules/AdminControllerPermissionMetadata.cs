using Store.Domain.Enums;

namespace Store.Api.Modules;

public sealed record AdminControllerPermissionMetadata(PermissionType PageType, PermissionType GroupType);