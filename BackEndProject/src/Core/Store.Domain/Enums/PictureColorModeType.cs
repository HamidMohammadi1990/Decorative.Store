using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum PictureColorModeType
{
    [Display(Name = "PictureColorModeType_RGB", ResourceType = typeof(EnumResources))]
    RGB = 1,

    [Display(Name = "PictureColorModeType_CMYK", ResourceType = typeof(EnumResources))]
    CMYK = 2,

    [Display(Name = "PictureColorModeType_All", ResourceType = typeof(EnumResources))]
    All = 3,

    [Display(Name = "PictureColorModeType_Unknown", ResourceType = typeof(EnumResources))]
    Unknown = 4
}
