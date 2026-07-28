using Edition.Application.Common.Utilities.JsonAttributes;
using Edition.Application.Models.Constants;

namespace Edition.Application.Common.Utilities.Security.Attributes;

public class ProductOrderItemAttachmentTypeEncryptor() : JsonIntEncryptor(SecurityKeyConstant.ProductOrderItemAttachmentType) { }
public class ProductOrderItemAttachmentTypeNullableEncryptor() : JsonNullableIntEncryptor(SecurityKeyConstant.ProductOrderItemAttachmentType) { }
