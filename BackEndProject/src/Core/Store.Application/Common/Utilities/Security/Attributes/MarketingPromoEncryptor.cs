using Edition.Application.Models.Constants;
using Edition.Application.Common.Utilities.JsonAttributes;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class MarketingPromoEncryptor() : JsonIntEncryptor(SecurityKeyConstant.MarketingPromo) { }

public class MarketingPromoNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.MarketingPromo) { }
