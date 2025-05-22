using KeyShieldAPI.Services;
using KeyShieldDB.Context;
using KeyShieldDB.Models;
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
    
    public async Task<Log> CreateLogAsync(LogDTOResponse logDto)
    {
        var log = new Log
        {
            Identifiant = logDto.Identifiant,
            UtilisateurCreateurIdentifiant = logDto.UtilisateurCreateurIdentifiant,
            Message = logDto.Message
        };

        await dbContext.Logs.AddAsync(log);
        await dbContext.SaveChangesAsync();
        return log;
    }
}