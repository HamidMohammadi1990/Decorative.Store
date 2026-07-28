using Edition.Application.Common.Utilities.JsonAttributes;
using Edition.Application.Models.Constants;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class DiscountEncryptor() : JsonIntEncryptor(SecurityKeyConstant.Discount) { }
public class DiscountNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.Discount) { }
