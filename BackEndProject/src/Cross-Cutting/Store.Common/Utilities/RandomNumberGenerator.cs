using System.Security.Cryptography;

namespace Store.Common.Utilities;

public static class NumberGenerator
{
    public static int Create()
        => RandomNumberGenerator.GetInt32(10_000, 100_000);

    public static int Create(int from, int to)
      => RandomNumberGenerator.GetInt32(from, to);
}