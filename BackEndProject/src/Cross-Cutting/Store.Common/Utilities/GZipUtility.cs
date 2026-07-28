using System.IO.Compression;

namespace Store.Common.Utilities;

public class GZipUtility
{
    public static byte[] Compress(string value)
    {
        using var outputStream = new MemoryStream();
        using var compressionStream = new GZipStream(outputStream, CompressionMode.Compress);
        using (var writer = new StreamWriter(compressionStream))
        {
            writer.Write(value);
        }
        return outputStream.ToArray();
    }

    public static string DeCompress(byte[] value)
    {
        using var compressedStream = new MemoryStream(value);
        using var decompressionStream = new GZipStream(compressedStream, CompressionMode.Decompress);
        using var reader = new StreamReader(decompressionStream);
        var decompressionValue = reader.ReadToEnd();
        return decompressionValue;
    }
}