using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class SectionItemEncryptor() : JsonIntEncryptor(SecurityKeyConstant.SectionItem) { }
public class SectionItemNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.SectionItem) { }
