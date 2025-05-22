using KeyShieldAPI.Repositories;
using KeyShieldAPI.Services.CoffreDeblocageService;
using KeyShieldDB.Models;
using KeyShieldDTO.RequestObjects;
using KeyShieldDTO.ResponseObjects;

namespace KeyShieldAPI.Services;

public class CoffreService(
    CoffreRepository coffreRepository,
    UtilisateurService utilisateurService,
    ICoffreDeblocageMemoryStore coffreDeblocageMemoryStore)
{
    public async Task<List<CoffreDTOResponse>> GetAllUtilisateurCoffresAsync()
    {
        var coffres = await coffreRepository.GetAllUtilisateurCoffresAsync();

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

        Console.WriteLine(
            $"Coffre : \n Nom : {coffre.Nom} \n Identifiant : {coffre.Identifiant} \n UtilisateurIdentifiant : {coffre.UtilisateurIdentifiant} \n DateCreation : {coffre.DateCreation}");

        await coffreRepository.CreateCoffreAsync(coffre);

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
        if (!hashComparison) return null;

        coffreDeblocageMemoryStore.RecordUnlock(coffreIdentifiant, coffreIdentifiant);
        return coffre.Sel;
    }

    public async Task<bool> DeleteCoffreAsync(string coffreId)
    {
        var coffreIdentifiant = Guid.TryParse(coffreId, out var result)
            ? result
            : throw new ArgumentException("Invalid Coffre ID format");

        var coffre = await coffreRepository.GetCoffreByIdAsync(coffreIdentifiant);

        if (coffre.UtilisateurIdentifiant != utilisateurService.CurrentAppUserId)
            throw new UnauthorizedAccessException("User does not have permission to delete this coffre");

        return await coffreRepository.DeleteCoffreAsync(coffreIdentifiant);
    }
}