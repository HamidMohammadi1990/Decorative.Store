using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class BankEncryptor() : JsonIntEncryptor(SecurityKeyConstant.Bank) { }
public class BankNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.Bank) { }