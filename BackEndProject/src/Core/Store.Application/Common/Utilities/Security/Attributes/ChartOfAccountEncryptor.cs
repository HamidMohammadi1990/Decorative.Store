using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class ChartOfAccountEncryptor() : JsonIntEncryptor(SecurityKeyConstant.ChartOfAccount) { }

public class ChartOfAccountNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.ChartOfAccount) { }