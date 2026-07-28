using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class CompanyCommentEncryptor() : JsonIntEncryptor(SecurityKeyConstant.CompanyComment) { }
public class CompanyCommentNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.CompanyComment) { }