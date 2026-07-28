using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class ProductPriceEncryptor() : JsonIntEncryptor(SecurityKeyConstant.ProductPrice) { }
public class ProductPriceNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.ProductPrice) { }