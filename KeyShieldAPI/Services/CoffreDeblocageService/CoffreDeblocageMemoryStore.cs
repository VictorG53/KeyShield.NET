namespace KeyShieldAPI.Services.CoffreDeblocageService;

public class CoffreDeblocageMemoryStore : ICoffreDeblocageMemoryStore
{
    private readonly List<CoffreDeblocage> _events = new();

    public IEnumerable<CoffreDeblocage> GetAll()
    {
        return _events;
    }

    public void RecordUnlock(Guid coffreId, Guid utilisateurId)
    {
        _events.Add(new CoffreDeblocage
        {
            CoffreIdentifiant = coffreId,
            UtilisateurIdentifiant = utilisateurId,
            UnlockedAt = DateTime.UtcNow
        });
    }
}