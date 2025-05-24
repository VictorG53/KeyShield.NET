using KeyShieldDB.Context;
using KeyShieldDB.Models;
using Microsoft.EntityFrameworkCore;

namespace KeyShieldAPI.Repositories;

public class UtilisateurRepository(KeyShieldDbContext dbContext)
{
    public async Task<Guid> GetOrCreateUtilisateurAsync(string entraId)
    {
        Utilisateur? utilisateur = await dbContext.Utilisateurs.FirstOrDefaultAsync(u => u.EntraId == entraId);
        if (utilisateur == null)
        {
            utilisateur = new Utilisateur
            {
                EntraId = entraId
            };
            dbContext.Utilisateurs.Add(utilisateur);
            await dbContext.SaveChangesAsync();
        }

        return utilisateur.Identifiant;
    }

    public async Task<Utilisateur?> GetUtilisateurByIdAsync(Guid utilisateurIdentifiant)
    {
        return await dbContext.Set<Utilisateur>().FindAsync(utilisateurIdentifiant);
    }
}