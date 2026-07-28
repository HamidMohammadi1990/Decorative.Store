using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class CompanyStoryCommentEncryptor() : JsonIntEncryptor(SecurityKeyConstant.CompanyStoryComment) { }
public class CompanyStoryCommentNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.CompanyStoryComment) { }
