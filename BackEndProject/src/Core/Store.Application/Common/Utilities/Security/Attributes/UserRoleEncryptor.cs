using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class UserRoleEncryptor() : JsonIntEncryptor(SecurityKeyConstant.UserRole) { }

public class UserRoleNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.UserRole) { }
