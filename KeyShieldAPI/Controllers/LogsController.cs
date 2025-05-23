using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyShieldAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/log")]
public class LogsController
{
    
}