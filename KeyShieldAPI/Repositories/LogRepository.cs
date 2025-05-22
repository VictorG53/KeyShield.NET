using KeyShieldAPI.Services;
using KeyShieldDB.Context;
using KeyShieldDB.Models;
using KeyShieldDTO.RequestObjects;
using KeyShieldDTO.ResponseObjects;
using Microsoft.EntityFrameworkCore;

namespace KeyShieldAPI.Repositories;

public class LogRepository(KeyShieldDbContext dbContext, UtilisateurService utilisateurService)
{
    public async Task CreateLogAsync(Log log)
    {
        await dbContext.Logs.AddAsync(log);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<List<Log>> GetAllLogsAsync()
    {
        return await dbContext.Logs.ToListAsync();
    }
    
    public async Task<Log> CreateLogAsync(LogDTORequest logDto)
    {
        // Récupérer l'utilisateur existant
        var utilisateur = await dbContext.Utilisateurs
            .FirstOrDefaultAsync(u => u.Identifiant == logDto.UtilisateurCreateurIdentifiant);

        // Récupérer l'action type existant
        var actionType = await dbContext.ActionTypes
            .FirstOrDefaultAsync(a => a.Identifiant == logDto.ActionTypeIdentifiant);

        if (utilisateur == null || actionType == null)
            throw new Exception("Utilisateur ou ActionType non trouvé.");

        var log = new Log
        {
            Identifiant = Guid.NewGuid(),
            Horodatage = DateTime.UtcNow,
            Message = logDto.Message,
            UtilisateurCreateurIdentifiant = utilisateur.Identifiant,
            ActionTypeIdentifiant = actionType.Identifiant,
        };

        try {
            await dbContext.Logs.AddAsync(log);
            await dbContext.SaveChangesAsync();
        } catch (Exception ex)
        {
            // Log ex.ToString() ou ex.InnerException?.Message
            throw;
        }
        return log;
    }
}