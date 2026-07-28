using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class ProductEncryptor(): JsonIntEncryptor(SecurityKeyConstant.Product) { }
public class ProductNullableEncryptor(): JsonNullableIntEncryptor(SecurityKeyConstant.Product) { }