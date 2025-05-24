using KeyShieldAPI.Services;
using KeyShieldDB.Context;
using KeyShieldDB.Models;
using Microsoft.EntityFrameworkCore;

namespace KeyShieldAPI.Repositories;

public class CoffreRepository(KeyShieldDbContext dbContext, UtilisateurService utilisateurService)
{
    public async Task<List<Coffre>> GetAllUtilisateurCoffresAsync()
    {
        return await dbContext.Coffres.Where(c => c.UtilisateurIdentifiant == utilisateurService.CurrentAppUserId)
            .ToListAsync();
    }

    public async Task<Coffre> GetCoffreByIdAsync(Guid identifiant)
    {
        Coffre coffre = await dbContext.Coffres.FirstOrDefaultAsync(c => c.Identifiant == identifiant) ??
                     throw new KeyNotFoundException($"Coffre with identifiant {identifiant} not found.");
        return coffre;
    }

    public async Task CreateCoffreAsync(Coffre coffre)
    {
        await dbContext.Coffres.AddAsync(coffre);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteCoffreAsync(Guid identifiant)
    {
        Coffre? coffre = await dbContext.Coffres.FirstOrDefaultAsync(c => c.Identifiant == identifiant);
        if (coffre == null) return false;
        coffre.DateSuppression = DateTime.UtcNow;
        dbContext.Coffres.Update(coffre);
        await dbContext.SaveChangesAsync();
        return true;
    }
}