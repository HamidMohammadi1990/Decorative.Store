using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class PageEncryptor() : JsonIntEncryptor(SecurityKeyConstant.Page) { }
public class PageNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.Page) { }
