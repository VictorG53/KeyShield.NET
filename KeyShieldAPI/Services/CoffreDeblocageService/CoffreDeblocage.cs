namespace KeyShieldAPI.Services.CoffreDeblocageService;

public class CoffreDeblocage
{
    public Guid CoffreIdentifiant { get; set; }
    public Guid UtilisateurIdentifiant { get; set; }
    public DateTime UnlockedAt { get; set; }
}