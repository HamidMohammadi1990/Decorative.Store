using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class PropertyCategoryEncryptor() : JsonIntEncryptor(SecurityKeyConstant.PropertyCategory) { }
public class PropertyCategoryNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.PropertyCategory) { }