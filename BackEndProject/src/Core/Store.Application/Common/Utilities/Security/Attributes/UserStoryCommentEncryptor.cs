using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class UserStoryCommentEncryptor() : JsonIntEncryptor(SecurityKeyConstant.UserStoryComment) { }
public class UserStoryCommentNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.UserStoryComment) { }
public class UserStoryLikeEncryptor() : JsonIntEncryptor(SecurityKeyConstant.UserStoryLike) { }
