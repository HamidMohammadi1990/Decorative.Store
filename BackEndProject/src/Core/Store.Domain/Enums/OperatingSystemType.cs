using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum OperatingSystemType
{
    [Display(Name = "OperatingSystemType_Unknown", ResourceType = typeof(EnumResources))]
    Unknown = 0,

    [Display(Name = "OperatingSystemType_Windows", ResourceType = typeof(EnumResources))]
    Windows = 1,

    [Display(Name = "OperatingSystemType_MacOs", ResourceType = typeof(EnumResources))]
    MacOs = 2,

    [Display(Name = "OperatingSystemType_Linux", ResourceType = typeof(EnumResources))]
    Linux = 3,

    [Display(Name = "OperatingSystemType_Android", ResourceType = typeof(EnumResources))]
    Android = 4,

    [Display(Name = "OperatingSystemType_iOS", ResourceType = typeof(EnumResources))]
    iOS = 5,

    [Display(Name = "OperatingSystemType_ChromeOs", ResourceType = typeof(EnumResources))]
    ChromeOs = 6
}
