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
        var coffre = await dbContext.Coffres.FirstOrDefaultAsync(c => c.Identifiant == identifiant) ??
                     throw new KeyNotFoundException($"Coffre with identifiant {identifiant} not found.");
        return coffre;
    }

    public async Task CreateCoffreAsync(Coffre coffre)
    {
        try
        {
            // Vérifiez ici que toutes les propriétés requises de 'coffre' sont bien renseignées
            Console.WriteLine($"UtilisateurIdentifiant: {coffre.UtilisateurIdentifiant}");

            await dbContext.Coffres.AddAsync(coffre);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            if (ex.InnerException != null) Console.WriteLine("InnerException: " + ex.InnerException.Message);
        }
    }

    public async Task<bool> DeleteCoffreAsync(Guid identifiant)
    {
        var coffre = await dbContext.Coffres.FirstOrDefaultAsync(c => c.Identifiant == identifiant);
        if (coffre == null) return false;
        coffre.DateSuppression = DateTime.UtcNow;
        dbContext.Coffres.Update(coffre);
        await dbContext.SaveChangesAsync();
        return true;
    }
}