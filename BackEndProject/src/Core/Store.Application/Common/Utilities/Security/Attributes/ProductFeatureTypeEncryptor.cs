using Edition.Application.Common.Utilities.JsonAttributes;
using Edition.Application.Models.Constants;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class ProductFeatureTypeEncryptor() : JsonIntEncryptor(SecurityKeyConstant.ProductFeatureType) { }
public class ProductFeatureTypeNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.ProductFeatureType) { }
