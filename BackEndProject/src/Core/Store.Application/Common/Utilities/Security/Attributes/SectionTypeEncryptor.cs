using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class SectionTypeEncryptor() : JsonIntEncryptor(SecurityKeyConstant.SectionType) { }
public class SectionTypeNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.SectionType) { }
