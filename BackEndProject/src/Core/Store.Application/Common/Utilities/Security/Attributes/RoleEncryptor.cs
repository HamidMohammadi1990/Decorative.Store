using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class RoleEncryptor() : JsonIntEncryptor(SecurityKeyConstant.Role) { }

public class RoleNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.Role) { }