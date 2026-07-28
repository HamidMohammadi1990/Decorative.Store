using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class CompanyStoryLikeEncryptor() : JsonIntEncryptor(SecurityKeyConstant.CompanyStoryLike) { }
public class CompanyStoryLikeNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.CompanyStoryLike) { }
