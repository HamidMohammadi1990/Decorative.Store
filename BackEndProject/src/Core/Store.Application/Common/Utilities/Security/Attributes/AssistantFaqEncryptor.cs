using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class AssistantFaqEncryptor() : JsonIntEncryptor(SecurityKeyConstant.AssistantFaq) { }

public class AssistantFaqNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.AssistantFaq) { }
