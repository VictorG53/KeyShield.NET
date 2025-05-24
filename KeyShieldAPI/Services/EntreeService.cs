using KeyShieldAPI.Repositories;
using KeyShieldAPI.Services.CoffreDeblocageService;
using KeyShieldDTO.RequestObjects;
using KeyShieldDTO.ResponseObjects;

namespace KeyShieldAPI.Services;

public class EntreeService(EntreeRepository entreeRepository, UtilisateurService utilisateurService, ICoffreDeblocageMemoryStore coffreDeblocageMemoryStore)
{

    public async Task CreateEntreeAsync(EntreeCreationDTORequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request), "Request cannot be null.");

        if (!coffreDeblocageMemoryStore.IsCoffreUnlocked(request.CoffreId, utilisateurService.CurrentAppUserId))
        {
            throw new UnauthorizedAccessException("Coffre is not unlocked.");
        }

        await entreeRepository.CreateEntreeAsync(request);

    }

    public async Task<List<EntreeDTOResponse>> GetAllCoffreEntreesAsync(string identifiant)
    {
        Guid coffreIdentifiant = Guid.TryParse(identifiant, out Guid result)
            ? result
            : throw new ArgumentException("Invalid Coffre ID format");

        if (string.IsNullOrEmpty(identifiant))
            throw new ArgumentException("Identifiant cannot be null or empty.", nameof(identifiant));

        if (!coffreDeblocageMemoryStore.IsCoffreUnlocked(coffreIdentifiant, utilisateurService.CurrentAppUserId))
        {
            throw new UnauthorizedAccessException("Coffre is not unlocked.");
        }


        return await entreeRepository.GetAllCoffreEntreesAsync(coffreIdentifiant);
    }

    public async Task<DonneeDTOResponse> GetMotDePasseAsync(string motDePasseIdentifiant)
    {
        if (string.IsNullOrEmpty(motDePasseIdentifiant))
            throw new ArgumentException("Entree ID cannot be null or empty.", nameof(motDePasseIdentifiant));

        Guid motDePasseIdentifiantGuid = Guid.TryParse(motDePasseIdentifiant, out Guid result)
            ? result
            : throw new ArgumentException("Invalid Entree ID format");

        Guid coffreIdentifiantGuid = await entreeRepository.GetCoffreIdFromDonneeIdAsync(motDePasseIdentifiantGuid);


        if (!coffreDeblocageMemoryStore.IsCoffreUnlocked(coffreIdentifiantGuid, utilisateurService.CurrentAppUserId))
        {
            throw new UnauthorizedAccessException("Coffre is not unlocked.");
        }

        return await entreeRepository.GetMotDePasseAsync(motDePasseIdentifiantGuid);
    }

}