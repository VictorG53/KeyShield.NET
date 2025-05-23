using KeyShieldAPI.Repositories;
using KeyShieldAPI.Services.CoffreDeblocageService;
using KeyShieldDB.Models;
using KeyShieldDTO.RequestObjects;
using KeyShieldDTO.ResponseObjects;
using ActionType = KeyShieldDTO.Const.ActionType;

namespace KeyShieldAPI.Services;

public class CoffreService(
    CoffreRepository coffreRepository,
    UtilisateurService utilisateurService,
    LogService logService,
    ICoffreDeblocageMemoryStore coffreDeblocageMemoryStore)
{
    public async Task<List<CoffreDTOResponse>> GetAllUtilisateurCoffresAsync()
    {
        var coffres = await coffreRepository.GetAllUtilisateurCoffresAsync();
        
        // Enregistrement des logs
        var log = new LogDTORequest();
        log.Identifiant = Guid.NewGuid();
        log.UtilisateurCreateurIdentifiant = utilisateurService.CurrentAppUserId;
        log.ActionTypeIdentifiant = Guid.Parse(ActionType.CREATE);
        log.HoroDatage = DateTime.Now;
        log.Message = "Affichage des coffres";
        
        await logService.CreateLogAsync(log);

        return coffres.Select(coffre => new CoffreDTOResponse
        {
            Identifiant = coffre.Identifiant,
            Nom = coffre.Nom
        }).ToList();
    }

    public async Task<byte[]> GetCoffreSaltAsync(string coffreId)
    {
        var coffreIdentifiant = Guid.TryParse(coffreId, out var result)
            ? result
            : throw new ArgumentException("Invalid Coffre ID format");

        var coffre = await coffreRepository.GetCoffreByIdAsync(coffreIdentifiant);

        return coffre.Sel;
    }

    public async Task<bool> GetAccess(string identifiant)
    {
        var coffreIdentifiant = Guid.TryParse(identifiant, out var result)
            ? result
            : throw new ArgumentException("Invalid Guid");
        var coffre = await coffreRepository.GetCoffreByIdAsync(coffreIdentifiant);
        
        // Enregistrement des logs
        var log = new LogDTORequest();
        log.Identifiant = Guid.NewGuid();
        log.UtilisateurCreateurIdentifiant = utilisateurService.CurrentAppUserId;
        log.ActionTypeIdentifiant = Guid.Parse(ActionType.CREATE);
        log.HoroDatage = DateTime.Now;
        log.Message = "Accès au coffre";
        await logService.CreateLogAsync(log);
        
        
        return true;
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

        Console.WriteLine(
            $"Coffre : \n Nom : {coffre.Nom} \n Identifiant : {coffre.Identifiant} \n UtilisateurIdentifiant : {coffre.UtilisateurIdentifiant} \n DateCreation : {coffre.DateCreation}");

        await coffreRepository.CreateCoffreAsync(coffre);
        
        // Enregistrement des logs
        var log = new LogDTORequest();
        log.Identifiant = Guid.NewGuid();
        log.UtilisateurCreateurIdentifiant = utilisateurService.CurrentAppUserId;
        log.ActionTypeIdentifiant = Guid.Parse(ActionType.CREATE);
        log.HoroDatage = DateTime.Now;
        log.Message = "Création d'un coffre";
        await logService.CreateLogAsync(log);

        return new CoffreDTOResponse
        {
            Identifiant = coffre.Identifiant,
            Nom = coffre.Nom
        };
    }

    public async Task<byte[]?> CheckPasswordAsync(string coffreId, byte[] passwordHash)
    {
        var coffreIdentifiant = Guid.TryParse(coffreId, out var result)
            ? result
            : throw new ArgumentException("Invalid Coffre ID format");

        var coffre = await coffreRepository.GetCoffreByIdAsync(coffreIdentifiant);

        if (coffre.UtilisateurIdentifiant != utilisateurService.CurrentAppUserId)
            throw new UnauthorizedAccessException("User does not have access to this coffre");

        var hashComparison = coffre.MotDePasseHash.SequenceEqual(passwordHash);
        if (hashComparison)
        {
            coffreDeblocageMemoryStore.RecordUnlock(coffreIdentifiant, coffreIdentifiant);
            return coffre.Sel;
        }

        return null;
    }

    public async Task<bool> DeleteCoffreAsync(string coffreId)
    {
        var coffreIdentifiant = Guid.TryParse(coffreId, out var result)
            ? result
            : throw new ArgumentException("Invalid Coffre ID format");

        // First get the coffre to verify ownership
        var coffre = await coffreRepository.GetCoffreByIdAsync(coffreIdentifiant);

        // Verify the current user is the owner of the coffre
        if (coffre.UtilisateurIdentifiant != utilisateurService.CurrentAppUserId)
            throw new UnauthorizedAccessException("User does not have permission to delete this coffre");
        
        
        // Enregistrement des logs
        var log = new LogDTORequest();
        log.Identifiant = Guid.NewGuid();
        log.UtilisateurCreateurIdentifiant = utilisateurService.CurrentAppUserId;
        log.ActionTypeIdentifiant = Guid.Parse(ActionType.CREATE);
        log.HoroDatage = DateTime.Now;
        log.Message = "Suppression du coffres";
        await logService.CreateLogAsync(log);
        
        // If ownership verification passed, proceed with deletion
        return await coffreRepository.DeleteCoffreAsync(coffreIdentifiant);
    }
}