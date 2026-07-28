using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum CompanyStoryMediaType
{
    [Display(Name = "CompanyStoryMediaType_Image", ResourceType = typeof(EnumResources))]
    Image = 1,

    [Display(Name = "CompanyStoryMediaType_Video", ResourceType = typeof(EnumResources))]
    Video = 2
}
