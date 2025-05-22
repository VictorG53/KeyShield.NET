namespace KeyShieldAPI.Services.CoffreDeblocageService;

public interface ICoffreDeblocageMemoryStore
{
    void RecordUnlock(Guid coffreId, Guid utilisateurId);
    IEnumerable<CoffreDeblocage> GetAll();

    bool IsCoffreUnlocked(Guid coffreId, Guid utilisateurId);
}