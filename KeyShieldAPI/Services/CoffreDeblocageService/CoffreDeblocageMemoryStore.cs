namespace KeyShieldAPI.Services.CoffreDeblocageService;

public class CoffreDeblocageMemoryStore : ICoffreDeblocageMemoryStore
{

    private readonly int minutesToExpire = 15;

    private readonly List<CoffreDeblocage> _events = new();

    public IEnumerable<CoffreDeblocage> GetAll()
    {
        return _events;
    }

    public void RecordUnlock(Guid coffreId, Guid utilisateurId)
    {
        Console.WriteLine($"Coffre {coffreId} unlocked by {utilisateurId} at {DateTime.UtcNow}");
        _events.Add(new CoffreDeblocage
        {
            CoffreIdentifiant = coffreId,
            UtilisateurIdentifiant = utilisateurId,
            UnlockedAt = DateTime.UtcNow
        });
    }

    public bool IsCoffreUnlocked(Guid coffreId, Guid utilisateurId)
    {
        // Nettoyage des événements expirés
        var expirationTime = DateTime.UtcNow.AddMinutes(-minutesToExpire);
        _events.RemoveAll(e => e.UnlockedAt < expirationTime);

        var unlockedEvent = _events.FirstOrDefault(e =>
            e.CoffreIdentifiant.Equals(coffreId) &&
            e.UtilisateurIdentifiant.Equals(utilisateurId) &&
            e.UnlockedAt > expirationTime
        );

        return unlockedEvent != null;
    }
}