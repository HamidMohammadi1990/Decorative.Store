using Store.Domain.Enums;

namespace Edition.Application.Models.Dtos;

public record ImageInspectorResultDto
{
    public int Width { get; set; }
    public int Height { get; set; }
    public double HorizontalResolution { get; set; }
    public double VerticalResolution { get; set; }
    public PictureColorModeType ColorMode { get; set; }
}