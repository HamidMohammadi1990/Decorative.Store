using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum FileSizeUnitType
{
    [Display(Name = "FileSizeUnitType_Bytes", ResourceType = typeof(EnumResources))]
    Bytes = 1,

    [Display(Name = "FileSizeUnitType_Kilobytes", ResourceType = typeof(EnumResources))]
    Kilobytes = 2,

    [Display(Name = "FileSizeUnitType_Megabytes", ResourceType = typeof(EnumResources))]
    Megabytes = 3,

    [Display(Name = "FileSizeUnitType_Gigabytes", ResourceType = typeof(EnumResources))]
    Gigabytes = 4
}
