using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class CityEncryptor() : JsonIntEncryptor(SecurityKeyConstant.City) { }

public class CityNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.City) { }