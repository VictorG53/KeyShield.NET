using KeyShieldAPI.Repositories;
using KeyShieldDB.Context;
using KeyShieldDTO.RequestObjects;
using KeyShieldDTO.ResponseObjects;
using Microsoft.EntityFrameworkCore;

namespace KeyShieldAPI.Services;

public class LogService(
    LogRepository logRepository, 
    UtilisateurService utilisateurService,
    KeyShieldDbContext dbContext
)
{
    public async Task<List<LogDTOResponse>> GetLogsAsync()
    {
        var logs = await logRepository.GetAllLogsAsync();
        var actionTypes = await dbContext.ActionTypes.ToListAsync();

        return logs.Select(log =>
        {
            var actionTypeLabel = actionTypes
                .FirstOrDefault(a => a.Identifiant == log.ActionTypeIdentifiant).Libelle ?? string.Empty;

            return new LogDTOResponse
            {
                Identifiant = log.Identifiant,
                HoroDatage = log.Horodatage,
                Message = log.Message,
                UtilisateurCreateurIdentifiant = log.UtilisateurCreateurIdentifiant,
                Action = actionTypeLabel,
            };
        }).ToList();

    }
    
    public async Task<LogDTORequest> CreateLogAsync(LogDTORequest logDto)
    {
        
        var log = await logRepository.CreateLogAsync(logDto);
        return logDto;
    }
}