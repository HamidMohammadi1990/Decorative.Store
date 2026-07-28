using Store.Domain.Enums;

namespace Edition.Application.Common.Utilities;

public class FileSizeConverter
{
    private const int BytesInKilobyte = 1024;
    private const int BytesInMegabyte = 1024 * 1024;
    private const int BytesInGigabyte = 1024 * 1024 * 1024;

    public static int Convert(int sizeInBytes, FileSizeUnitType targetUnit)
    {
        return targetUnit switch
        {
            FileSizeUnitType.Bytes => sizeInBytes,
            FileSizeUnitType.Kilobytes => sizeInBytes / BytesInKilobyte,
            FileSizeUnitType.Megabytes => sizeInBytes / BytesInMegabyte,
            FileSizeUnitType.Gigabytes => sizeInBytes / BytesInGigabyte,
            _ => throw new ArgumentException("Invalid target unit")
        };
    }

    public static long ConvertToBytes(double size, FileSizeUnitType sourceUnit)
    {
        return sourceUnit switch
        {
            FileSizeUnitType.Bytes => (long)size,
            FileSizeUnitType.Kilobytes => (long)(size * BytesInKilobyte),
            FileSizeUnitType.Megabytes => (long)(size * BytesInMegabyte),
            FileSizeUnitType.Gigabytes => (long)(size * BytesInGigabyte),
            _ => throw new ArgumentException("Invalid source unit")
        };
    }
}