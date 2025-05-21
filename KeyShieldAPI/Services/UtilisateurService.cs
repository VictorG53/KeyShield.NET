using KeyShieldAPI.Repositories;
using KeyShieldDTO.ResponseObjects;

namespace KeyShieldAPI.Services;

public class UtilisateurService(UtilisateurRepository utilisateurRepository)
{

    public Guid CurrentAppUserId { get; set; }

    public async Task<Guid> GetOrCreateUtilisateurAsync(string externalUserId) => await utilisateurRepository.GetOrCreateUtilisateurAsync(externalUserId);
}