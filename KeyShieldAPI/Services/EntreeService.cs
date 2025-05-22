using KeyShieldAPI.Repositories;
using KeyShieldAPI.Services.CoffreDeblocageService;
using KeyShieldDTO.RequestObjects;

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

}