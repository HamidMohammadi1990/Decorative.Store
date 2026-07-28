using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class ProductDescriptionEncryptor() : JsonIntEncryptor(SecurityKeyConstant.ProductDescription) { }
public class ProductDescriptionNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.ProductDescription) { }