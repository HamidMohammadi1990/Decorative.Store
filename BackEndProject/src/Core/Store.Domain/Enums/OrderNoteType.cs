using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum OrderNoteType
{
    [Display(Name = "OrderNoteType_Private", ResourceType = typeof(EnumResources))]
    Private = 1,

    [Display(Name = "OrderNoteType_General", ResourceType = typeof(EnumResources))]
    General = 2,

    [Display(Name = "OrderNoteType_Systemic", ResourceType = typeof(EnumResources))]
    Systemic = 3
}
