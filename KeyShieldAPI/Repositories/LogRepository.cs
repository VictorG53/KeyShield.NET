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
        Console.WriteLine("Log enregistré");

        var log = new Log
        {
            Identifiant = Guid.NewGuid(),
            Horodatage = DateTime.UtcNow,
            Message = logDto.Message,
        };
        
        return log;
    }
}