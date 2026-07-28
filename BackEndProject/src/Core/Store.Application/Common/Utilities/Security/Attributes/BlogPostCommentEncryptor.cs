using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class BlogPostCommentEncryptor() : JsonIntEncryptor(SecurityKeyConstant.BlogPostComment) { }
public class BlogPostCommentNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.BlogPostComment) { }