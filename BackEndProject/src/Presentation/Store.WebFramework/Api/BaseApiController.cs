using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Store.WebFramework.Api;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseApiController : ControllerBase { }

[Authorize]
[ApiController]
[Area("admin")]
[Route("api/v{version:apiVersion}/[area]/[controller]")]
public class BaseApiAdminController : ControllerBase { }