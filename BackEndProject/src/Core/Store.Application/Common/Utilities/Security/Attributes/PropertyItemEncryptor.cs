using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class PropertyItemEncryptor(): JsonIntEncryptor(SecurityKeyConstant.PropertyItem) { }
public class PropertyItemNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.PropertyItem) { }