using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class ProductQuestionEncryptor() : JsonIntEncryptor(SecurityKeyConstant.ProductQuestion) { }

public class ProductQuestionNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.ProductQuestion) { }
