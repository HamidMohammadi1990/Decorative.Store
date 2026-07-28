using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class BlogPostEncryptor(): JsonIntEncryptor(SecurityKeyConstant.BlogPost) { }

public class BlogPostNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.BlogPost) { }