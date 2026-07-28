using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class CompanyStoryEncryptor() : JsonIntEncryptor(SecurityKeyConstant.CompanyStory) { }
public class CompanyStoryNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.CompanyStory) { }
