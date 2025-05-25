using KeyShieldAPI.Repositories;
using KeyShieldDB.Context;
using KeyShieldDB.Models;
using KeyShieldDTO.RequestObjects;
using KeyShieldDTO.ResponseObjects;
using Microsoft.EntityFrameworkCore;

namespace KeyShieldAPI.Services;

public class LogService(
    LogRepository logRepository,
    KeyShieldDbContext dbContext
)
{
    public async Task<List<LogDTOResponse>> GetLogsAsync()
    {
        List<Log> logs = await logRepository.GetAllLogsAsync();
        List<ActionType> actionTypes = await dbContext.ActionTypes.ToListAsync();

        return logs.Select(log =>
        {
            var actionTypeLabel = actionTypes?
                .FirstOrDefault(a => a.Identifiant == log.ActionTypeIdentifiant)?.Libelle ?? string.Empty;

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

    public async Task<LogDTOResponse> CreateLogAsync(LogDTORequest logDto)
    {

        Log log = await logRepository.CreateLogAsync(logDto);

        return new LogDTOResponse()
        {
            Identifiant = log.Identifiant,
            HoroDatage = log.Horodatage,
            Message = log.Message,
            UtilisateurCreateurIdentifiant = log.UtilisateurCreateurIdentifiant,
            Action = log.ActionType.Libelle ?? string.Empty
        }
        ;
    }
}