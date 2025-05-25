using KeyShieldAPI.Services;
using KeyShieldDTO.ResponseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyShieldAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/log")]
public class LogsController(LogService logService) : ControllerBase
{
    public async Task<List<LogDTOResponse>> GetAllLogs()
    {
        List<LogDTOResponse> logs = await logService.GetLogsAsync();

        return logs;
    }
}