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
        // Récupération du type d'action
        var actionType = await dbContext.ActionTypes
            .FirstOrDefaultAsync(a => a.Identifiant == logDto.ActionTypeIdentifiant);

        if (actionType == null)
            throw new Exception("Type d'action non trouvée");

        // Récupération de l'utilisateur
        var user = await dbContext.Utilisateurs
            .FirstOrDefaultAsync(u => u.Identifiant == logDto.UtilisateurCreateurIdentifiant);

        if (user == null)
            throw new Exception("Utilisateur introuvable");
        
        // Console.WriteLine(user.EntraId);
        

        var log = new Log
        {
            Identifiant = Guid.NewGuid(),
            // Horodatage = new DateTime(),
            Horodatage = DateTime.Now,
            Message = logDto.Message,
            UtilisateurCreateurIdentifiant = logDto.UtilisateurCreateurIdentifiant,
            UtilisateurPartageIdentifiant = null,
            ActionTypeIdentifiant = logDto.ActionTypeIdentifiant,
            CoffreIdentifiant = null,
            EntreeIdentifiant = null,
            DonneeIdentifiant = null,
            Utilisateur = user,
            UtilisateurPartage = null,
            Coffre = null,
            Entree = null,
            Donnee = null
        };
        
        await dbContext.Logs.AddAsync(log);
        await dbContext.SaveChangesAsync();
        
        return log;
    }
}