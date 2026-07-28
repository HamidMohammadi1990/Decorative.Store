using Store.Domain.Enums;

namespace Store.Domain.Dtos.Products;

public record ProductOrderAttachmentDto
{
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
    public bool IsRequired { get; init; }
    public int ProductOrderItemAttachmentTypeId { get; init; }
    public decimal MinWidth { get; init; }
    public decimal MaxWidth { get; init; }
    public decimal MinHeight { get; init; }
    public decimal MaxHeight { get; init; }
    public int MinHorizontalResolution { get; init; }
    public int MaxHorizontalResolution { get; init; }
    public int MinVerticalResolution { get; init; }
    public int MaxVerticalResolution { get; init; }
    public PictureColorModeType ColorMode { get; init; }
    public int MaxFileSizeInBytes { get; init; }
}