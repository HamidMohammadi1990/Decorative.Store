using Store.Domain.Enums;

namespace Store.Api.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ControllerInfoAttribute : Attribute
{
    public PermissionType PermissionType { get; }
    public PermissionType GroupType { get; }

    public ControllerInfoAttribute(PermissionType groupType)
    {
        GroupType = groupType;
        PermissionType = groupType;
    }

    public ControllerInfoAttribute(PermissionType permissionType, PermissionType groupType)
    {
        PermissionType = permissionType;
        GroupType = groupType;
    }
}