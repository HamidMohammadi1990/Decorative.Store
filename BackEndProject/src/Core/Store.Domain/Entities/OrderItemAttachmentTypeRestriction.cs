using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class OrderItemAttachmentTypeRestriction : BaseEntity
{
    public int ProductOrderItemAttachmentTypeId { get; private set; }
    public bool IsRequired { get; private set; }
    public decimal MinWidth { get; private set; }
    public decimal MaxWidth { get; private set; }
    public decimal MinHeight { get; private set; }
    public decimal MaxHeight { get; private set; }
    public int MinHorizontalResolution { get; private set; }
    public int MaxHorizontalResolution { get; private set; }
    public int MinVerticalResolution { get; private set; }
    public int MaxVerticalResolution { get; private set; }
    public PictureColorModeType ColorMode { get; private set; }
    public int MaxFileSizeInBytes { get; private set; }


    public ProductOrderItemAttachmentType ProductOrderItemAttachmentType { get; private set; } = default!;


    public static OrderItemAttachmentTypeRestriction Create
          (int productOrderItemAttachmentTypeId, decimal minWidth, decimal maxWidth, decimal minHeight, decimal maxHeight,
           int minHorizontalResolution, int maxHorizontalResolution, int minVerticalResolution, int maxVerticalResolution,
           PictureColorModeType colorMode, bool isRequired, int maxFileSizeInBytes)
        => new()
        {
            MinWidth = minWidth,
            MaxWidth = maxWidth,
            MinHeight = minHeight,
            MaxHeight = maxHeight,
            ColorMode = colorMode,
            IsRequired = isRequired,
            MaxFileSizeInBytes = maxFileSizeInBytes,
            MinVerticalResolution = minVerticalResolution,
            MaxVerticalResolution = maxVerticalResolution,
            MinHorizontalResolution = minHorizontalResolution,
            MaxHorizontalResolution = maxHorizontalResolution,
            ProductOrderItemAttachmentTypeId = productOrderItemAttachmentTypeId
        };
}