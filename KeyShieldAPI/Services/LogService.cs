using KeyShieldAPI.Repositories;
using KeyShieldDB.Models;
using KeyShieldDTO.RequestObjects;
using KeyShieldDTO.ResponseObjects;


namespace KeyShieldAPI.Services;

public class LogService(
    LogRepository logRepository,
    ActionTypeRepository actionTypeRepository
)
{
    public async Task<List<LogDTOResponse>> GetLogsAsync()
    {
        List<Log> logs = await logRepository.GetAllLogsAsync();
        List<ActionType> actionTypes = await actionTypeRepository.GetListActionType();

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