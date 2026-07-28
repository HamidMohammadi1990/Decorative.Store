using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class OrderEncryptor(): JsonIntEncryptor(SecurityKeyConstant.Order) { }

public class OrderNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.Order) { }