using System.Net;
using Store.Common.Enums;

namespace Store.Common.Exceptions;

public sealed class AppContentPolicyAccessDeniedException()
    : AppException(OperationStatusCode.NotFound, "AccessDenied", HttpStatusCode.NotFound);