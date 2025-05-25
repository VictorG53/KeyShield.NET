using KeyShieldAPI.Repositories;
using KeyShieldAPI.Services.CoffreDeblocageService;
using KeyShieldDB.Models;
using KeyShieldDTO.RequestObjects;
using KeyShieldDTO.ResponseObjects;
using ActionType = KeyShieldDTO.Constants.ActionType;

namespace KeyShieldAPI.Services;

public class CoffreService(
    CoffreRepository coffreRepository,
    UtilisateurService utilisateurService,
    LogService logService,
    ICoffreDeblocageMemoryStore coffreDeblocageMemoryStore)
{
    public async Task<List<CoffreDTOResponse>> GetAllUtilisateurCoffresAsync()
    {
        List<Coffre> coffres = await coffreRepository.GetAllUtilisateurCoffresAsync();

        // Enregistrement des logs
        await logService.CreateLogAsync(new(
            "Affichage de tous les coffres de l'utilisateur",
            utilisateurService.CurrentAppUserId,
            ActionType.READ
        ));

        return coffres.Select(coffre => new CoffreDTOResponse
        {
            Identifiant = coffre.Identifiant,
            Nom = coffre.Nom
        }).ToList();
    }

    public async Task<CoffreDTOResponse> CreateCoffreAsync(CoffreDTORequest request)
    {
        Coffre coffre = new()
        {
            Nom = request.Nom,
            Identifiant = Guid.NewGuid(),
            Sel = request.Sel,
            UtilisateurIdentifiant = utilisateurService.CurrentAppUserId,
            MotDePasseHash = request.MotDePasseHash,
            DateCreation = DateTime.Now
        };

        // Enregistrement des logs
        await logService.CreateLogAsync(new(
            "Création d'un coffre",
            utilisateurService.CurrentAppUserId,
            ActionType.CREATE
        ));

        await coffreRepository.CreateCoffreAsync(coffre);

        return new CoffreDTOResponse
        {
            Identifiant = coffre.Identifiant,
            Nom = coffre.Nom
        };
    }

    public bool IsCoffreUnlocked(string coffreId)
    {
        if (string.IsNullOrWhiteSpace(coffreId))
            throw new ArgumentException("Coffre ID cannot be null or empty.", nameof(coffreId));

        Guid coffreIdentifiant = Guid.TryParse(coffreId, out Guid result)
            ? result
            : throw new ArgumentException("Invalid Coffre ID format");

        return coffreDeblocageMemoryStore.IsCoffreUnlocked(coffreIdentifiant, utilisateurService.CurrentAppUserId);
    }


    public async Task<byte[]?> CheckPasswordAsync(string coffreId, byte[] passwordHash)
    {
        Guid coffreIdentifiant = Guid.TryParse(coffreId, out Guid result)
            ? result
            : throw new ArgumentException("Invalid Coffre ID format");

        Coffre coffre = await coffreRepository.GetCoffreByIdAsync(coffreIdentifiant);

        if (coffre.UtilisateurIdentifiant != utilisateurService.CurrentAppUserId)
            throw new UnauthorizedAccessException("User does not have access to this coffre");

        bool hashComparison = coffre.MotDePasseHash.SequenceEqual(passwordHash);
        if (!hashComparison)
        {
            await logService.CreateLogAsync(new(
                "Tentative de déblocage de coffre échouée",
                utilisateurService.CurrentAppUserId,
                ActionType.READ
            ));
            return null;
        }

        await logService.CreateLogAsync(new(
            "Tentative de déblocage de coffre réussie",
            utilisateurService.CurrentAppUserId,
            ActionType.READ
        ));

        coffreDeblocageMemoryStore.RecordUnlock(coffreIdentifiant, utilisateurService.CurrentAppUserId);
        return coffre.Sel;
    }

    public async Task<bool> DeleteCoffreAsync(string coffreId)
    {
        Guid coffreIdentifiant = Guid.TryParse(coffreId, out Guid result)
            ? result
            : throw new ArgumentException("Invalid Coffre ID format");

        Coffre coffre = await coffreRepository.GetCoffreByIdAsync(coffreIdentifiant);

        if (coffre.UtilisateurIdentifiant != utilisateurService.CurrentAppUserId)
            throw new UnauthorizedAccessException("User does not have permission to delete this coffre");

        // Enregistrement des logs
        await logService.CreateLogAsync(new(
            "Suppression d'un coffre",
            utilisateurService.CurrentAppUserId,
            ActionType.DELETE
        ));

        return await coffreRepository.DeleteCoffreAsync(coffreIdentifiant);
    }
}