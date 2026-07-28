using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class CommentTopicEncryptor() : JsonIntEncryptor(SecurityKeyConstant.CommentTopic) { }

public class CommentTopicNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.CommentTopic) { }