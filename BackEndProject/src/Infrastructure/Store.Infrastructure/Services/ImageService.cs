using SixLabors.ImageSharp;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats;
using Edition.Application.Models.Dtos;
using SixLabors.ImageSharp.Formats.Jpeg;
using Edition.Application.Contracts.Infrastructure;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Store.Infrastructure.Services;

public class ImageService : IImageService
{
    public async Task<OperationResult<ImageInspectorResultDto>> GetImageMetaDataAsync(IFormFile file)
    {
        try
        {
            string fileExtention = Path.GetExtension(file.FileName);
            string[] acceptableImageExtentions = [".jpg", ".jpeg"];
            if (!acceptableImageExtentions.Contains(fileExtention))
                return ErrorModel.Create("InvalidFileExtension");

            using var memoryStream = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var imageDecoder = JpegDecoder.Instance;
            var options = new DecoderOptions { Configuration = Configuration.Default };
            var image = imageDecoder.Decode(options, memoryStream);
            if (image is null)
                return ErrorModel.Create("InvalidFile");

            int width = image.Width;
            int height = image.Height;
            var horizontalResolution = image.Metadata.HorizontalResolution > 0 ? image.Metadata.HorizontalResolution : 72;
            var verticalResolution = image.Metadata.VerticalResolution > 0 ? image.Metadata.VerticalResolution : 72;

            var colorMode = PictureColorModeType.Unknown;
            var jpegColorMode = image.Metadata.GetJpegMetadata().ColorType;
            if (jpegColorMode.HasValue)
            {
                colorMode = jpegColorMode switch
                {
                    JpegEncodingColor.Rgb => PictureColorModeType.RGB,
                    JpegEncodingColor.Cmyk => PictureColorModeType.CMYK,
                    _ => PictureColorModeType.Unknown
                };
            }

            return new ImageInspectorResultDto
            {
                Width = width,
                Height = height,
                ColorMode = colorMode,
                VerticalResolution = verticalResolution,
                HorizontalResolution = horizontalResolution
            };
        }
        catch (Exception ex)
        {
            return ErrorModel.CreateLiteral("ImageProcessingFailed", ex.Message);
        }
    }
}