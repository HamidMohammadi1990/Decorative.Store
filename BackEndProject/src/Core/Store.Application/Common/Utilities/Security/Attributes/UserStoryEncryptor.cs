using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class UserStoryEncryptor() : JsonIntEncryptor(SecurityKeyConstant.UserStory) { }
public class UserStoryNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.UserStory) { }
