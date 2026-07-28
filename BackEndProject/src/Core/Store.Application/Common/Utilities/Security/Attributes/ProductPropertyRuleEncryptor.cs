using Edition.Application.Common.Utilities.JsonAttributes;
using Edition.Application.Models.Constants;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class ProductPropertyRuleEncryptor() : JsonIntEncryptor(SecurityKeyConstant.ProductPropertyRule) { }
public class ProductPropertyRuleNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.ProductPropertyRule) { }
