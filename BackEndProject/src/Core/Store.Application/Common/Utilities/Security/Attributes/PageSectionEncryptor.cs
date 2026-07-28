using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class PageSectionEncryptor() : JsonIntEncryptor(SecurityKeyConstant.PageSection) { }
public class PageSectionNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.PageSection) { }
