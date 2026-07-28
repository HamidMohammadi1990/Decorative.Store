using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class SubCategoryEncryptor() : JsonIntEncryptor(SecurityKeyConstant.SubCategory) { }

public class SubCategoryNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.SubCategory) { }