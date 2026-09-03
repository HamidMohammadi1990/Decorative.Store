using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class RoomTypeEncryptor() : JsonIntEncryptor(SecurityKeyConstant.RoomType) { }

public class RoomTypeNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.RoomType) { }
