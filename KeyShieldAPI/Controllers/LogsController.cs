using KeyShieldAPI.Services;
using KeyShieldDTO.ResponseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyShieldAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/log")]
public class LogsController(LogService logService, UtilisateurService utilisateurService) : ControllerBase
{
    public async Task<List<LogDTOResponse>> GetAllLogs()
    {
        Console.WriteLine("Je passe par ici");
        var logs = await logService.GetLogsAsync();

        return logs;
    }
}