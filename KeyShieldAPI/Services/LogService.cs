using KeyShieldAPI.Repositories;
using KeyShieldDB.Models;
using KeyShieldDTO.ResponseObjects;

namespace KeyShieldAPI.Services;

public class LogService(LogRepository logRepository, UtilisateurService utilisateurService)
{
    public async Task<List<LogDTOResponse>> GetLogsAsync()
    {
        var logs = await logRepository.GetAllLogsAsync();
        return logs.Select(log => new LogDTOResponse
        {
            Identifiant = log.Identifiant,
            UtilisateurCreateurIdentifiant = log.UtilisateurCreateurIdentifiant,
            Message = log.Message,
        }).ToList();
    }
    
    public async Task<LogDTOResponse> CreateLogAsync(LogDTOResponse logDto)
    {
        var log = await logRepository.CreateLogAsync(logDto);
        return new LogDTOResponse
        {
            Identifiant = log.Identifiant,
            UtilisateurCreateurIdentifiant = log.UtilisateurCreateurIdentifiant,
            Message = log.Message
        };
    }
}