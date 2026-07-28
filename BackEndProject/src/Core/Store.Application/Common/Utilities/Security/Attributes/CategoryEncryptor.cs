using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class CategoryEncryptor() : JsonIntEncryptor(SecurityKeyConstant.Category) { }

public class CategoryNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.Category) { }